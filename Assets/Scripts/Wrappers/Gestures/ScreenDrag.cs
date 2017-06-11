using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenDrag : IGesture {

	private float dragSpeed = 2;
	private Vector3 previousMousePosition;
	public ScreenDrag () { }

	public event GestureFinishHandler OnGestureFinish = () => { };
	
	public bool Condition (Vector3 mousePosition, RaycastHit2D[] colliders) {
		GameObject collider = null;
		for (int i = 0; i < colliders.Length; i++ ) {
			if (colliders[i].collider.gameObject.GetComponent<ISquadView>() != null) {
				collider = colliders[i].collider.gameObject;
			}
		}
		previousMousePosition = mousePosition;
		return Input.GetMouseButton(0) && collider == null && !EventSystem.current.IsPointerOverGameObject();
	}
    
	public void Update (Vector3 mousePosition) {
		Vector3 direction = (mousePosition - previousMousePosition).normalized * Time.deltaTime * dragSpeed;
		Camera.main.transform.position = new Vector3(
			Camera.main.transform.position.x + direction.x,
			Camera.main.transform.position.y + direction.y,
			Camera.main.transform.position.z
		);
		previousMousePosition = mousePosition;
		if (Input.GetMouseButtonUp(0)) OnGestureFinish();
	}
}
