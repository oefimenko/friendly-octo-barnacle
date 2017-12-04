using UnityEngine;

public delegate void SquadPathDrawnHandler (IPath path);

public class PathDraw : IGesture {

	private IPath currentPath;
	private GameObject gui;

	public PathDraw () {
		gui = GameObject.FindGameObjectWithTag("GUI");
	}

	public event GestureFinishHandler OnGestureFinish = () => { };
	public event SquadPathDrawnHandler OnPathDrawFinish = delegate { };
	
	public bool Condition (Vector3 mousePosition, RaycastHit2D[] colliders) {
		GameObject squad = null;
		for (int i = 0; i < colliders.Length; i++ ) {
			if (colliders[i].collider.gameObject.GetComponent<ISquadView>() != null) {
				GameObject gobject = colliders[i].collider.gameObject;
				if (SquadMonitor.Instance.Get(gobject.name).Owner == GameManager.Instance.UserName) {
					squad = colliders[i].collider.gameObject;
					currentPath = new RenderedPath(mousePosition);
					gui.SetActive(false);
				}
				
			}
		}
		return Input.GetMouseButton(0) && squad != null;
	}
    
	public void Update (Vector3 mousePosition) {
		if (Input.GetMouseButtonUp (0)) {
			currentPath.Complete(mousePosition);
			OnPathDrawFinish (currentPath);
			currentPath = null;
			gui.SetActive (true);
			OnGestureFinish ();
		} else {
			currentPath.Update(mousePosition);
		}
		
	}
}
