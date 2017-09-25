
public static class Endpoints {

	private static string server1 = "127.0.0.1";
	private static string server2 = "146.148.12.90";

	private static int httpPort = 8880;
	private static int listenPort = 21000;
	private static int serverPort;
	private static string hash;

	private static string server = server1;

	public static int ListenPort { get { return listenPort; } }
	public static int ServerPort { get { return serverPort; } set { serverPort = value; } }
	public static string ServerIP { get { return server; } }
	public static string HTTPEndpoint { get { 
			return "http://" + server + ":" + httpPort.ToString(); 
		}
	}
	public static string Hash { get { return hash; } set { hash = value; } }

}
