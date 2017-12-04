using System.Text;

public class Serializer {

	public Serializer () {}

	public byte[] Serialize (Connection msg) {
		string toBytes = ";" + msg.SquadName;
		byte[] result = new byte[toBytes.Length + 2];
		new byte[] { 0x01, 0x00 }.CopyTo(result, 0);
		Encoding.ASCII.GetBytes (toBytes).CopyTo (result, 2);
		return result;
	}

	public byte[] Serialize (FormationChangedMessage msg) {
		string toBytes = ";" + msg.SquadName + ";" + msg.Formation + ";" + msg.NewVersion.ToString();
		byte[] result = new byte[toBytes.Length + 2];
		new byte[] { 0x02, 0x03 }.CopyTo(result, 0);
		Encoding.ASCII.GetBytes (toBytes).CopyTo (result, 2);
		return result;
	}

	public byte[] Serialize (PathAssignedMessage msg) {
		string toBytes = ";" + msg.SquadName + ";" + msg.Path.ToString() + ";" + msg.NewVersion.ToString();
		byte[] result = new byte[toBytes.Length + 2];
		new byte[] { 0x02, 0x02 }.CopyTo(result, 0);
		Encoding.ASCII.GetBytes (toBytes).CopyTo (result, 2);
		return result;
	}

}
