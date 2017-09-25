using System.Collections;
using UnityEngine;

public delegate void UnitStopHandler ();
public delegate void UnitPositionChangeHandler (Vector2 newPosition);

public interface IUnitView {

    event UnitPositionChangeHandler OnPositionChange;
    event UnitStopHandler OnUnitStop;
    void Navigate(Vector2 aim, Speed speed);
    void SetState (int state);
    void Destroy();
}

public class UnitView : MonoBehaviour, IUnitView {

    private Animator animator;

    // Events
    public event UnitPositionChangeHandler OnPositionChange = (Vector2 newPosition) => { };
    public event UnitStopHandler OnUnitStop = () => { };

    void Awake () {
        animator = gameObject.GetComponent<Animator>();
    }

    public void SetState (int state) {
        animator.SetInteger("State", state);
		animator.SetFloat("Seed", Random.value);
    }

    public void Navigate(Vector2 aim, Speed speed) {
        StopAllCoroutines();
        StartCoroutine(Walk(aim, speed));
    }

    IEnumerator Walk(Vector2 point, Speed speed) {
        Vector3 aim = new Vector3(point.x, point.y, transform.position.z);
        //yield return StartCoroutine(Rotate(aim, speed.Rotatation, 180f));
        StartCoroutine(Rotate(aim, speed.Rotatation));
        while (Vector3.Distance(transform.position, aim) >= 0.05) {
            Vector3 vector = (aim - transform.position).normalized;
            transform.Translate(vector * speed.Movement * Time.deltaTime, Space.World);
            yield return null;
            OnPositionChange(transform.position);
        }
        OnUnitStop();
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
        animator = null;
        GameObject.Destroy(this.gameObject);
    }
}
