using UnityEngine;

public class InputManager {

    private IGesture currentGesture;
    private CleanClick cleanClick;
    private PathDraw pathDraw;
    private ScreenDrag screenDrag;
    private ScreenResize screenResize;
    private SquadSelect squadSelect;
    // events
    public event SquadClickHandler OnSquadClicked = delegate { };
    public event SquadPathDrawnHandler OnSquadPathDrawn = delegate { };
    
    
    public InputManager () {
        cleanClick = new CleanClick();
        pathDraw = new PathDraw();
        screenDrag = new ScreenDrag();
        screenResize = new ScreenResize();
        squadSelect = new SquadSelect();
        
        UEventsManager.Instance.OnLateUpdate += Update;
        cleanClick.OnGestureFinish += CleanGestureHandler;
        pathDraw.OnGestureFinish += CleanGestureHandler;
        screenDrag.OnGestureFinish += CleanGestureHandler;
        screenResize.OnGestureFinish += CleanGestureHandler;
        squadSelect.OnGestureFinish += CleanGestureHandler;

        cleanClick.OnCleanClick += (string name) => OnSquadClicked(name);
        squadSelect.OnSquadClick += (string name) => OnSquadClicked(name);
        pathDraw.OnPathDrawFinish += (Path path) => OnSquadPathDrawn(path);
    }

    public void Destroy () {
        UEventsManager.Instance.OnLateUpdate -= Update;
        cleanClick.OnGestureFinish -= CleanGestureHandler;
        pathDraw.OnGestureFinish -= CleanGestureHandler;
        screenDrag.OnGestureFinish -= CleanGestureHandler;
        screenResize.OnGestureFinish -= CleanGestureHandler;
        squadSelect.OnGestureFinish -= CleanGestureHandler;
        cleanClick.OnCleanClick -= OnSquadClicked;
        squadSelect.OnSquadClick -= OnSquadClicked;
        cleanClick = null;
        pathDraw = null;
        screenDrag = null;
        screenResize = null;
        squadSelect = null;
        GameObject.Destroy(GameObject.FindGameObjectWithTag("GUI"));
    }

    private void Update () {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (currentGesture != null) {
            currentGesture.Update(mousePosition);
        } else {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] colliders = Physics2D.GetRayIntersectionAll(ray);
            if (screenResize.Condition(mousePosition, colliders))
            {
                currentGesture = screenResize;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (cleanClick.Condition(mousePosition, colliders)) currentGesture = cleanClick;
                else if (squadSelect.Condition(mousePosition, colliders)) currentGesture = squadSelect;
            }
            else if (Input.GetMouseButton(0))
            {
                if (screenDrag.Condition(mousePosition, colliders)) currentGesture = screenDrag;
                else if (pathDraw.Condition(mousePosition, colliders)) currentGesture = pathDraw;
            }
        }
    }

    private void CleanGestureHandler() {
        currentGesture = null;
    }

}
