//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

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

    private GameManager () {
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
    }

}
