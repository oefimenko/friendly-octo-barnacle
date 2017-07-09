
public static class Endpoints {

	private static string server1 = "127.0.0.1";
	private static string server2 = "35.190.198.159";


	private static string server = server2;
	private static int port = 8880;

	private static int listenPort = 21000;

	public static int ListenPort { get { return listenPort; } }
	public static string ServerIP { get { return server; } }
	public static string HTTPEndpoint { get { 
			return "http://" + server + ":" + port.ToString(); 
		}
	}

}
