using UnityEngine;

public delegate void SquadPathDrawnHandler (Path path);

public class PathDraw : IGesture {

	private Path currentPath;
	private GameObject gui;

	public PathDraw () {
		gui = GameObject.FindGameObjectWithTag("GUI");
	}

	public event GestureFinishHandler OnGestureFinish = () => { };
	public event SquadPathDrawnHandler OnPathDrawFinish = delegate { };
	
	public bool Condition (Vector3 mousePosition, RaycastHit2D[] colliders) {
		GameObject collider = null;
		for (int i = 0; i < colliders.Length; i++ ) {
			if (colliders[i].collider.gameObject.GetComponent<ISquadView>() != null) {
				collider = colliders[i].collider.gameObject;
				currentPath = new Path(mousePosition);
				gui.SetActive(false);
			}
		}
		return Input.GetMouseButton(0) && collider != null;
	}
    
	public void Update (Vector3 mousePosition) {
		currentPath.Update(mousePosition);
		if (Input.GetMouseButtonUp(0)) {
			OnPathDrawFinish(currentPath);
			currentPath = null;
			gui.SetActive(true);
			OnGestureFinish();
		}
		
	}
}
