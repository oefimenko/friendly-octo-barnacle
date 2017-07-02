using System.Collections.Generic;

public class PathsHandler {

	private static PathsHandler instance;
	public static PathsHandler Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}
			else
			{
				instance = new PathsHandler();
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

	private Dictionary<string, IPath> paths;

	public PathsHandler () {
		paths = new Dictionary<string, IPath>();
	}

	public IPath Get (string name) {
		IPath result;
		paths.TryGetValue(name, out result);
		return result;
	}

	public void Add (string name, IPath path) {
		paths[name] = path;
	}

	public void Remove (string name) {
		paths.Remove(name);
	}

	public void Clear () {
		List<string> keyList = new List<string>(paths.Keys);
		foreach (string key in keyList) {
			paths[key].Destroy();
		}
		paths.Clear();
	}
}
