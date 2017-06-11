using UnityEngine;

public delegate void GestureFinishHandler ();

public interface IGesture {

    void Update (Vector3 mousePosition);
    bool Condition (Vector3 mousePosition, RaycastHit2D[] colliders);
    event GestureFinishHandler OnGestureFinish;

}
