using System.Collections.Generic;

public class SquadMonitor {

    private static SquadMonitor instance;
    public static SquadMonitor Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = new SquadMonitor();
                return instance;
            }
        }
    }

    public static void Purge () {
        if (instance != null) {
            instance.Clear();
            instance = null;
        }
    }

    private Dictionary<string, ISquadModel> squads;

    private SquadMonitor () {
        squads = new Dictionary<string, ISquadModel>();
    }
    
    public ISquadModel Get (string name) {
        return squads[name];
    }

    public void Add (string name, ISquadModel squad) {
        squads[name] = squad;
    }

    public void Remove (string name) {
        squads.Remove(name);
    }

    public void Clear () {
        foreach (KeyValuePair <string, ISquadModel> pair in squads) {
            pair.Value.Destroy();
        }
        squads.Clear();
    }
}
