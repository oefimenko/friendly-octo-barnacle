using UnityEngine;

public class SquadController {

    private ISquadModel model;
    private ISquadView view;

    public SquadController(ISquadModel nModel, ISquadView nView) {
        model = nModel;
        view = nView;
        // Subscriptions
        model.OnPathSet += PathChange;
        model.OnDestroy += Destroy;
        model.OnBoundsSet += BoundsChanged;
        view.OnPositionChange += PositionChange;
        view.OnPointReached += NextPoint;
    }

    public void Destroy () {
        model.OnPathSet -= PathChange;
        model.OnDestroy -= Destroy;
        view.OnPositionChange -= PositionChange;
        view.OnPointReached -= NextPoint;
        view.Destroy();
        model = null;
        view = null;
    }

    private void PathChange () {
        NextPoint ();
    }

    private void PositionChange (Vector2 newPosition, Quaternion rotation) {
        model.Postion = newPosition;
        model.Rotation = rotation;
    }

    private void NextPoint () {
        Vector2? aim = model.Path.NextPoint();
        if (aim != null) {
            view.Navigate((Vector2)aim, model.Speed);
            model.LocalAim = (Vector2)aim;
        } else {
            model.Path = null;
        }
    }
    
    private void BoundsChanged (Vector2 bounds) {
        view.SetBounds(bounds);
    }
}
