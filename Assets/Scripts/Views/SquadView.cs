using System.Collections;
using UnityEngine;

public delegate void PointReachedHandler ();
public delegate void SquadPositionChangeHandler(Vector2 newPosition, Quaternion rotation);

public interface ISquadView {

    event SquadPositionChangeHandler OnPositionChange;
    event PointReachedHandler OnPointReached;
    void Navigate(Vector2 point, Speed speed);
    void Destroy();
    void SetBounds(Vector2 bounds);
}

public class SquadView : MonoBehaviour, ISquadView {

    private BoxCollider2D objectCollider;

    // Events
    public event SquadPositionChangeHandler OnPositionChange = (Vector2 newPosition, Quaternion rotation) => { };
    public event PointReachedHandler OnPointReached = () => { };

    void Awake () {
        objectCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    public void Navigate (Vector2 point, Speed speed) {
        StopAllCoroutines();
        StartCoroutine(Move(point, speed));
    }

    IEnumerator Move (Vector2 point, Speed speed) {
        Vector3 aim = new Vector3(point.x, point.y, transform.position.z);
        //yield return StartCoroutine(Rotate(aim, speed.Rotatation, 180f));
        StartCoroutine(Rotate(aim, speed.Rotatation));
        while (Vector3.Distance(transform.position, aim) >= 0.05) {
            Vector3 vector = (aim - transform.position).normalized;
            transform.Translate(vector * speed.Movement * Time.deltaTime, Space.World);
            yield return null;
            OnPositionChange(transform.position, transform.rotation);
        }
        OnPointReached();
    }

    IEnumerator Rotate (Vector3 point, float speed, float sens = 1f) {
        if (point == transform.position) yield break;
        Vector3 direction = (point - transform.position).normalized;
        Quaternion targetRotation = Utils.SafeQuaternion(direction, Vector3.forward);
        float treshold = 1 - sens / 2 / 180;

        while (-Vector3.Dot(direction, transform.up) <= treshold) {
            targetRotation = Utils.SafeQuaternion(direction, Vector3.forward);
            targetRotation.x = 0.0f;
            targetRotation.y = 0.0f;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
            yield return null;
            direction = (point - transform.position).normalized;
        }
    }

    public void Destroy () {
        objectCollider = null;
        GameObject.Destroy(this.gameObject);
    }

    public void SetBounds(Vector2 bounds) {
        objectCollider.size = bounds;
    }
}
