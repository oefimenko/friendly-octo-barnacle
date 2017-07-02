using System.Text;

public class Serializer {

	public Serializer () {}

	public byte[] Serialize (FormationChangedMessage msg) {
		string toBytes = ";" + msg.SquadName + ";" + msg.Formation;
		byte[] result = new byte[toBytes.Length + 1];
		byte id = 0x03;
		result.SetValue(id, 0);
		Encoding.ASCII.GetBytes (toBytes).CopyTo (result, 1);
		return result;
	}

	public byte[] Serialize (PathAssignedMessage msg) {
		string toBytes = ";" + msg.SquadName + ";" + msg.Path.ToString();
		byte[] result = new byte[toBytes.Length + 1];
		byte id = 0x02;
		result.SetValue(id, 0);
		Encoding.ASCII.GetBytes (toBytes).CopyTo (result, 1);
		return result;
	}

}
