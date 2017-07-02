using System.Collections.Generic;
using UnityEngine;

public class SlientPath : IPath {

	private List<Vector3> path = new List<Vector3>();
	private string name;

	public SlientPath () { }

	public void Update (Vector3 position) {
		path.Add(position);
	}

	public void Complete (Vector3 position) {
		Update(position);
		int x1 = (int)path[0].x * 1000;
		int y1 = (int)path[0].y * 1000;
		int x2 = (int)path[path.Count - 1].x * 1000;
		int y2 = (int)path[path.Count - 1].y * 1000;
		name = x1.ToString() + y1.ToString() + x2.ToString() + y2.ToString();
		PathsHandler.Instance.Add(name, this);
	}

	public Vector2? NextPoint() {
		if (path.Count >= 1) {
			Vector3 point3 = path[0];
			path.RemoveAt(0);
			return new Vector2(point3.x, point3.y);
		} 
		return null;
	}

	public void Destroy () {
		path.Clear();
		PathsHandler.Instance.Remove(name);
	}

	public override string ToString () {
		string result = "";
		for (int i = 0; i < path.Count; i++) {
			string x = ((int)(path[i].x * 1000)).ToString();
			string y = ((int)(path[i].y * 1000)).ToString();
			result += x + ":" + y + ":";
		}
		return result.TrimEnd(':');
	}
}
