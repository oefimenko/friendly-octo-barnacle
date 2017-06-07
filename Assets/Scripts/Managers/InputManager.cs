using System;
using UnityEngine;

public delegate void SquadClickedHandler (string name);
public delegate void SquadPathDrawnHandler (Path path);

public class InputManager {

    private GameObject gui;
    private Path currentPath;
    private SquadModel currentSquad;
    private Action<Vector3> inputHandler;
    // Refactor related functionality
    private float dragSpeed = 2;
    private Vector3 previousMousePosition;
    private Path path;
    // events
    public event SquadClickedHandler OnSquadClicked;
    public event SquadPathDrawnHandler OnSquadPathDrawn;

    public InputManager () {
        gui = GameObject.FindGameObjectWithTag("GUI");
        UEventsManager.Instance.OnLateUpdate += Update;
    }

    public void Destroy () {
        UEventsManager.Instance.OnLateUpdate -= Update;
        currentPath = null;
        currentSquad = null;
        inputHandler = null;
        GameObject.Destroy(gui);
    }

    private void Update () {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (inputHandler != null) {
            inputHandler(mousePosition);
        } else if (Input.GetMouseButtonDown(0)) {
            previousMousePosition = mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] colliders = Physics2D.GetRayIntersectionAll(ray);
            GameObject collider = null;
            for (int i = 0; i < colliders.Length; i++ ) {
                if (colliders[i].collider.gameObject.GetComponent<ISquadView>() != null) {
                    collider = colliders[i].collider.gameObject;
                }
            }
            if (collider != null) {
                inputHandler = InteractWithSquad;
                OnSquadClicked(collider.transform.gameObject.name);
                path = new Path(collider.transform.position);
                InteractWithSquad(mousePosition);
            } else {
                inputHandler = DragScreen;
                DragScreen(mousePosition);
            }
        }
    }

    private void InteractWithSquad(Vector3 position) {
        path.Update(position);
        if (Input.GetMouseButtonUp(0)) {
            inputHandler = null;
            path.Complete(position);
            OnSquadPathDrawn(path);
        }
    }

    private void DragScreen (Vector3 position) {
        Vector3 direction = (position - previousMousePosition).normalized * Time.deltaTime * dragSpeed;
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x + direction.x,
            Camera.main.transform.position.y + direction.y,
            Camera.main.transform.position.z
        );
        previousMousePosition = position;
        if (Input.GetMouseButtonUp(0)) inputHandler = null;
    }

}
