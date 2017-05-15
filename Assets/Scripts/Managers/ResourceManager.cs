using System.Collections.Generic;
using UnityEngine;

public class ResourceManager {

    private static ResourceManager instance;
    public static ResourceManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = new ResourceManager();
                return instance;
            }
        }
    }

    public static void Purge()
    {
        if (instance != null)
        {
            instance.Clear();
            instance = null;
        }
    }


    private Dictionary<string, GameObject> resources;
    
    private ResourceManager () {
        resources = new Dictionary<string, GameObject>();
    }

    public void Clear () {
        resources.Clear();
        Resources.UnloadUnusedAssets();
    }

    public GameObject Get (string type, string name) {
        GameObject result;
        if (resources.ContainsKey(name))
        {
            result = resources[name];
        }
        else
        {
            result = Resources.Load(type + "/" + name) as GameObject;
            resources[name] = result;
        }
        return result;
    }

}
