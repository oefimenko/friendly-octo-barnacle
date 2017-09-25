using System;

public class GameTime {

	private static GameTime instance;
	public static GameTime Instance {
		get
		{
			if (instance != null)
			{
				return instance;
			}
			else
			{
				instance = new GameTime();
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

	long time;
	long t;

	public GameTime () {
		time = (long)(DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1)).TotalMilliseconds * 1000);
		UEventsManager.Instance.OnUpdate += Update;
	}

	public void SetTime (long time) {
		this.time = time;
	}

	public long Time () {
		return time;
	}

	public string Timestamp () {
		return time.ToString();
	}

	public void Destroy () {
		UEventsManager.Instance.OnUpdate -= Update;
	}

	private void Update () {
		time += (long)(UnityEngine.Time.deltaTime * 1000000);
	}
}

