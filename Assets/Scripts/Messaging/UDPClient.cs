using System;  
using System.Net;  
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;

public class UDPClient {

	public static byte[] SubBytes(byte[] bytes, int start, int length) {
		byte[] result = new byte[length];
		Array.Copy(bytes, start, result, 0, length);
		return result;
	}

	public struct Message {
		public byte[] raw;
		public byte type;
		public byte ack;
		public int method;
		public string stamp;
		public string hash;
		public string body;

		public Message(byte[] bytes) {
			raw = bytes;
			type = bytes[0];
			ack = bytes[1];
			stamp = Encoding.ASCII.GetString(bytes, 2, 16);
			if (type == 0) {
				hash = Encoding.ASCII.GetString(bytes, 18, 16);
				method = bytes[34] * 10 + bytes[35];
				body = Encoding.ASCII.GetString(bytes, 36, bytes.Length - 36);
			} else {
				hash = null;
				method = -1;
				body = null;
			}
		}

		public override string ToString() {
			return "Type: " + type.ToString() +
			"; ACK: " + ack.ToString() +
			"; STAMP: " + stamp +
			"; HASH: " + hash +
			"; METHOD: " + method.ToString() +
			"; BODY: " + body;
		}
	}

	private UdpClient updClient;
	private IPAddress ip;
	private string hash;
	private IPEndPoint readEP;
	private IPEndPoint sendEP;
	private int selfPort;
	private int serverPort;
	private volatile bool listen;
	private Dictionary<string, byte[]> messagesSent = new Dictionary<string, byte[]>();

	private Deserializer deserializer;
	private Serializer serializer;

	public UDPClient (int port, string serverIP, int serverPort, string hash) {
		listen = true;
		selfPort = port;
		updClient = new UdpClient(selfPort);

		ip = IPAddress.Parse (serverIP);
		this.serverPort = serverPort;
		this.hash = hash;
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
		messagesSent.Clear();

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
				Message msg = new Message(bytes);
//				UnityEngine.Debug.Log("Received from " + readEP.ToString() + " : " + Encoding.ASCII.GetString(bytes,2,bytes.Length - 2));
				UnityEngine.Debug.Log("Received: " + msg.ToString());
				CheckIncoming(msg);
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

	private void CheckIncoming (Message msg) {
		if (msg.type == 0 && msg.ack == 1)
		{
			Acknowledge (msg.raw);
			ProcessIncoming(msg);
		} 
		else if (msg.type == 0 && msg.ack == 0)
		{
			ProcessIncoming(msg);
		} 
		else if ((msg.type == 1 && msg.ack == 0) || (msg.type == 0 && msg.ack == 1))
		{
			messagesSent.Remove(msg.stamp);
		} 
		else
		{
			UnityEngine.Debug.Log ("UNKNOWN MESSAGE TYPE");
		}
	}

	private void ProcessIncoming (Message msg) {
		long stamp = Int64.Parse (msg.stamp);
		switch (msg.method)
		{
		case 11:
			break;
		case 12:
			long time = stamp + Int64.Parse (msg.body);
			GameTime.Instance.SetTime (time);
			break;
		case 13:
			GameSyncQueue.Instance.QueueEvent (deserializer.ParseInit (stamp, msg.body));
			break;
		case 14:
//			GameSyncQueue.Instance.QueueEvent(deserializer.ParseInit(stamp, msg.body));
			break;
		case 21:
//			GameSyncQueue.Instance.QueueEvent(deserializer.ParseState(stamp, body));
			break;
		case 22:
//			GameSyncQueue.Instance.QueueEvent(deserializer.ParsePath(stamp, msg.body));
			break;
		case 23:
//			GameSyncQueue.Instance.QueueEvent(deserializer.ParseFormation(stamp, msg.body));
			break;
		case 24:
//			GameSyncQueue.Instance.QueueEvent(deserializer.ParseSkill(stamp, msg.body));
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
		byte[] result = new byte[34 + bytes.Length];
		result [0] = 0;
		result [1] = 1;
		string timestamp = GameTime.Instance.Timestamp();
		Encoding.ASCII.GetBytes(timestamp).CopyTo(result, 2);
		Encoding.ASCII.GetBytes(hash).CopyTo (result, 18);
		bytes.CopyTo(result, 34);
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
