using UnityEngine;
using UnityEngine.EventSystems;

public class CleanClick : IGesture {

	public CleanClick () { }

	public event GestureFinishHandler OnGestureFinish = () => { };
	public event SquadClickHandler OnCleanClick = delegate { };

	public bool Condition (Vector3 mousePosition, RaycastHit2D[] colliders) {
		GameObject collider = null;
		for (int i = 0; i < colliders.Length; i++ ) {
			if (colliders[i].collider.gameObject.GetComponent<ISquadView>() != null) {
				collider = colliders[i].collider.gameObject;
			}
		}
		return collider == null && !EventSystem.current.IsPointerOverGameObject();
	}

	public void Update (Vector3 mousePosition) {
		OnCleanClick("");
		OnGestureFinish();
	}
}
