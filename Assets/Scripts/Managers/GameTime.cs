using System;

public class GameTime {

	public static string Timestamp () {
		string timestamp = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString();
		return timestamp + new String('0', (16 - timestamp.Length));
	}
	
}

