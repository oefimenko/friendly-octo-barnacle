//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
using System;
using System.Threading;

public class GameManager {

    private static GameManager instance;
    public static GameManager Instance {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = new GameManager();
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
	private HTTPClient client;
	private UDPClient gameUDPClient;
	private Thread gameUDPClientThread;

    private GameManager () {
        StartMessageClients();
        control = new InputManager();
        SquadUIControllFactory.Create(control);

		GameSyncQueue.Instance.AddListener<InitMessage>(OnInit);

		client = new HTTPClient(Endpoints.HTTPEndpoint);
		client.Login("test_user");
		client.Training ();
    }

    public void Destroy () {
        ResourceManager.Purge();
        control.Destroy();
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
        
		gameUDPClient = new UDPClient(Endpoints.ListenPort, Endpoints.ServerIP); 
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
