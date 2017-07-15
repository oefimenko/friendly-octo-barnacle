using System;  
using System.Net;  
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;

public class UDPClient {

	private UdpClient updClient;
	private IPAddress ip;
	private IPEndPoint readEP;
	private IPEndPoint sendEP;
	private int selfPort;
	private int serverPort;
	private volatile bool listen;
	private Dictionary<string, byte[]> messagesSent = new Dictionary<string, byte[]>();

	private Deserializer deserializer;
	private Serializer serializer;

	public UDPClient (int port, string serverIP, int serverPort) {
		listen = true;
		selfPort = port;
		updClient = new UdpClient(selfPort);

		ip = IPAddress.Parse (serverIP);
		this.serverPort = serverPort;
		sendEP = new IPEndPoint(ip, this.serverPort);

		readEP = new IPEndPoint(IPAddress.Any, port);

		deserializer = new Deserializer();
		serializer = new Serializer();

		GameMessageQueue.Instance.AddListener<PathAssignedMessage>(PathAssigned);
		GameMessageQueue.Instance.AddListener<FormationChangedMessage>(FormationChanged);
		GameMessageQueue.Instance.AddListener<SkillUsedMessage>(SkillUsed);

		SendWithHeaders(serializer.Serialize(new Connection(GameManager.Instance.UserName)));
	}

    public void Destroy () {
		listen = false;
		updClient.Close();

		GameMessageQueue.Instance.RemoveListener<PathAssignedMessage>(PathAssigned);
		GameMessageQueue.Instance.RemoveListener<FormationChangedMessage>(FormationChanged);
		GameMessageQueue.Instance.RemoveListener<SkillUsedMessage>(SkillUsed);
	}

	public void StartListener () {
		try   
		{  
			while (listen)   
			{
				byte[] bytes = updClient.Receive(ref readEP);
				UnityEngine.Debug.Log("Received from " + readEP.ToString() + " : " + Encoding.ASCII.GetString(bytes,2,bytes.Length - 2));
				CheckIncoming(bytes);
			}  
		}   
		catch (Exception e)
		{
			UnityEngine.Debug.Log(e.ToString());
			updClient.Close();
			updClient = new UdpClient(selfPort);
			StartListener ();
		}  
	}

	private void CheckIncoming (byte[] bytes) {
		if (bytes[0] == 0 && bytes[1] == 1)
		{
			ProcessIncoming (Encoding.ASCII.GetString(bytes, 18, bytes.Length - 18));
			Acknowledge (bytes);
		} 
		else if (bytes[0] == 0 && bytes[1] == 0)
		{
			ProcessIncoming (Encoding.ASCII.GetString(bytes, 18, bytes.Length - 18));
		} 
		else if ((bytes[0] == 1 && bytes[1] == 0) || (bytes[0] == 0 && bytes[1] == 1))
		{
			messagesSent.Remove(Encoding.ASCII.GetString(bytes, 2, 16));
		} 
		else 
		{
			UnityEngine.Debug.Log ("UNKNOWN MESSAGE TYPE");
		}
	}

	private void ProcessIncoming (string body) {
		int type = Int16.Parse (body[0].ToString() + body[1].ToString());
		switch (type)
		{
		case 12:
			GameSyncQueue.Instance.QueueEvent(deserializer.ParseInit (body));
			break;
		case 21:
//			GameSyncQueue.Instance.QueueEvent(deserializer.ParseState(body));
			break;
		case 22:
			GameSyncQueue.Instance.QueueEvent(deserializer.ParsePath(body));
			break;
		case 23:
			GameSyncQueue.Instance.QueueEvent(deserializer.ParseFormation(body));
			break;
		case 24:
//			GameSyncQueue.Instance.QueueEvent(deserializer.ParseSkill(body));
			break;
		default:
			break;
		}
	}

	private void PathAssigned (PathAssignedMessage msg) {
		SendWithHeaders(serializer.Serialize(msg));
	}

	private void FormationChanged (FormationChangedMessage msg) {
		SendWithHeaders(serializer.Serialize(msg));
	}

	private void SkillUsed (SkillUsedMessage msg) {
		//        gameSync.SkillUsed(msg);
	}

	private void SendWithHeaders (byte[] bytes) {
		byte[] result = new byte[18 + bytes.Length];
		result [0] = 0;
		result [1] = 1;
		string timestamp = GameTime.Timestamp();
		Encoding.ASCII.GetBytes(timestamp).CopyTo(result, 2);
		bytes.CopyTo(result, 18);
		messagesSent[timestamp] = result;
		updClient.Send(result, result.Length, sendEP);

		AddAckCheckTimer (timestamp);
	}

	private void Acknowledge (byte[] bytes) {
		byte[] stamp = SubBytes(bytes, 2, 16);
		bytes = new byte[18];
		(new byte[] {1, 0}).CopyTo (bytes, 0);
		stamp.CopyTo (bytes, 2);
		updClient.Send(bytes, bytes.Length, sendEP);
	}

	private byte[] SubBytes(byte[] bytes, int start, int length) {
		byte[] result = new byte[length];
		Array.Copy(bytes, start, result, 0, length);
		return result;
	}

	private void AddAckCheckTimer (string timestamp) {
		System.Threading.Timer timer = null; 
		timer = new Timer((obj) =>
			{
				CheckAck(timestamp);
				timer.Dispose();
			}, 
			null, 500, Timeout.Infinite);
	}

	private void CheckAck (string timestamp) {
		if (messagesSent.ContainsKey (timestamp)) {
			updClient.Send (messagesSent[timestamp], messagesSent[timestamp].Length, sendEP);
			AddAckCheckTimer(timestamp);
		} else {
			messagesSent.Remove(timestamp);
		}
	}
}
