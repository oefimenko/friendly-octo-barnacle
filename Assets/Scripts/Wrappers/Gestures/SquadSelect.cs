using UnityEngine;

public delegate void SquadClickHandler (string name);

public class SquadSelect : IGesture {

    private string lastSquad;
    public SquadSelect () { }

    public event GestureFinishHandler OnGestureFinish = () => { };
    public event SquadClickHandler OnSquadClick = delegate { };
	
    public bool Condition (Vector3 mousePosition, RaycastHit2D[] colliders) {
        GameObject collider = null;
        for (int i = 0; i < colliders.Length; i++ ) {
            if (colliders[i].collider.gameObject.GetComponent<ISquadView>() != null) {
                collider = colliders[i].collider.gameObject;
                lastSquad = collider.name;
            }
        }
        return collider != null;
    }
    
    public void Update (Vector3 mousePosition) {
        OnSquadClick(lastSquad);
        OnGestureFinish();
    }
    
}
