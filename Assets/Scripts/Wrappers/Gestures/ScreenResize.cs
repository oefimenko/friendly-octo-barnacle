using UnityEngine;

public class ScreenResize : IGesture {
    
    private float scrollSpeed = 2;
    public ScreenResize () { }

    public event GestureFinishHandler OnGestureFinish = () => { };
	
    public bool Condition (Vector3 mousePosition, RaycastHit2D[] colliders) {
        return Input.GetAxis("Mouse ScrollWheel") != 0f;
    }
    
    public void Update (Vector3 mousePosition) {
        Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        if (Input.GetAxis("Mouse ScrollWheel") == 0f) OnGestureFinish();
    }
}
