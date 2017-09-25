using System;
using System.Threading;

public class MatchManager {

	private static MatchManager instance;
	public static MatchManager Instance {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
				instance = new MatchManager();
                return instance;
            }
        }
    }

    public static void Purge () {
        if (instance != null)
        {
            instance.Destroy();
            instance = null;
        }
    }

    private InputManager control;
	private UDPClient gameUDPClient;
	private Thread gameUDPClientThread;

    private MatchManager () {
        StartMessageClients();
        control = new InputManager();
        SquadUIControllFactory.Create(control);
		GameTime stub = GameTime.Instance;
    }

    public void Destroy () {
        ResourceManager.Purge();
        control.Destroy();
		GameTime.Instance.Destroy();
        // Destroy all path
		PathsHandler.Purge();
        UEventsManager.Purge();
        control = null;
        
		// Messaging
		gameUDPClient.Destroy();
		gameUDPClient = null;
		gameUDPClientThread.Abort();
        GameMessageQueue.Purge();
        GameSyncQueue.Purge();
		GameSyncQueue.Instance.RemoveListener<InitMessage>(OnInit);
    }

    private void StartMessageClients () {
        var stub1 = GameMessageQueue.Instance;
        var stub2 = GameSyncQueue.Instance;
        
		GameSyncQueue.Instance.AddListener<InitMessage>(OnInit);

		gameUDPClient = new UDPClient(Endpoints.ListenPort, Endpoints.ServerIP, Endpoints.ServerPort, Endpoints.Hash);
		ThreadStart childrefSync = () => { 
			gameUDPClient.StartListener();
		};
		gameUDPClientThread = new Thread(childrefSync);
		gameUDPClientThread.IsBackground = true;
		gameUDPClientThread.Start();
    }
    
	private void OnInit (InitMessage msg) {
		for (int i = 0; i < msg.Squads.Length; i++) {
			SquadFactory.Create(
				msg.Squads[i].Type, msg.Squads[i].Name, msg.Squads[i].Side, msg.Squads[i].Postion
			);
		}
	}

}
