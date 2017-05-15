﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PointReachedHandler ();
public delegate void SquadPositionChangeHandler(Vector2 newPosition);

public interface ISquadView {

    event SquadPositionChangeHandler OnPositionChange;
    event PointReachedHandler OnPointReached;
    void Navigate(Vector2 point, Speed speed);
    void Destroy();

}

public class SquadView : MonoBehaviour, ISquadView {

    private BoxCollider2D objectCollider;

    // Events
    public event SquadPositionChangeHandler OnPositionChange = (Vector2 newPosition) => { };
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
        yield return StartCoroutine(Rotate(aim, speed.Rotatation, 15f));
        StartCoroutine(Rotate(aim, speed.Rotatation));
        while (Vector3.Distance(transform.position, aim) >= 0.05)
        {
            Vector3 vector = (aim - transform.position).normalized;
            transform.Translate(vector * speed.Movement * Time.deltaTime, Space.World);
            yield return null;
            OnPositionChange(transform.position);
        }
        OnPointReached();
    }

    IEnumerator Rotate (Vector3 point, float speed, float sens = 1f) {
        Vector3 direction = (point - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.forward);
        float treshold = 1 - sens / 2 / 180;
        while (-Vector3.Dot(direction, transform.up) <= treshold)
        {
            targetRotation = Quaternion.LookRotation(direction, Vector3.forward);
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
}
