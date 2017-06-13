//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
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
    private GameMessageClient gameMessageClient;
    private GameSyncClient gameSyncClient;
    private Thread gameSyncClientThread;
    private Thread gameMessageClientThread;

    private GameManager () {
        StartMessageClients();
        
        control = new InputManager();
        SquadFactory.Create("SkeletonSquad", "SkeletonSquad1", 0, new UnityEngine.Vector2(-2, -2));
        SquadFactory.Create("SpiderSquad", "SpiderSquad1", 0, new UnityEngine.Vector2(2, 2));
        SquadUIControllFactory.Create(control);
    }

    public void Destroy () {

        ResourceManager.Purge();
        control.Destroy();
        // Destroy all path
        UEventsManager.Purge();
        control = null;
        // Messaging
        gameSyncClient.Destroy();
        gameMessageClient.Destroy();
        gameSyncClient = null;
        gameMessageClient = null;
        
        gameSyncClientThread.Abort();
        gameMessageClientThread.Abort();
        
        GameMessageQueue.Purge();
        GameSyncQueue.Purge();
    }

    private void StartMessageClients () {
        var stub1 = GameMessageQueue.Instance;
        var stub2 = GameSyncQueue.Instance;
        
        
        ThreadStart childrefSync = () => { gameSyncClient = new GameSyncClient(); };
        gameSyncClientThread = new Thread(childrefSync);
        gameSyncClientThread.Start();
        
        ThreadStart childrefGame = () => { gameMessageClient = new GameMessageClient(gameSyncClient); };
        gameMessageClientThread = new Thread(childrefGame);   
        gameMessageClientThread.Start();
    }
    
}
