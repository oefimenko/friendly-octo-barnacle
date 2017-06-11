using UnityEngine;

public class UEventsManager : MonoBehaviour {

    public delegate void EventHandler();

    private static UEventsManager manager;
    static public UEventsManager Instance {
        get {
            if (manager != null) {
                return manager;
            }
            else {
                manager = GameObject.FindGameObjectWithTag("UEventsManager").GetComponent<UEventsManager>();
                return manager;
            }
        }
    }

    public static void Purge () {
        GameObject.Destroy(manager.gameObject);
    }

    // Unity Events
    public event EventHandler OnAwake = delegate { };
    public event EventHandler OnFixedUpdate = delegate { };
    public event EventHandler OnStart = delegate { };
    public event EventHandler OnUpdate = delegate { };
    public event EventHandler OnLateUpdate = delegate { };

    void Awake () {
        OnAwake();
        var stub = GameManager.Instance;
    }

    void Start () {
        OnStart();
	}
	
	void Update () {
        OnUpdate();
    }

    void FixedUpdate () {
        OnFixedUpdate();
    }

    void LateUpdate () {
        OnLateUpdate();
    }
}
