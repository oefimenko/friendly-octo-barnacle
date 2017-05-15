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
    public event EventHandler OnAwake = () => { };
    public event EventHandler OnFixedUpdate = () => { };
    public event EventHandler OnStart = () => { };
    public event EventHandler OnUpdate = () => { };
    public event EventHandler OnLateUpdate = () => { };

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
