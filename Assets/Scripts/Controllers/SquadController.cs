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
        view.OnPositionChange += PositionChange;
        view.OnPointReached += PointReached;
    }

    public void Destroy () {
        model.OnPathSet -= PathChange;
        model.OnDestroy -= Destroy;
        view.OnPositionChange -= PositionChange;
        view.OnPointReached -= PointReached;
        view.Destroy();
        model = null;
        view = null;
    }

    private void PathChange (Path path, Speed speed) {
        //view.Navigate(path., speed);
    }

    private void PositionChange (Vector2 newPosition) {
        model.Postion = newPosition;
    }

    private void PointReached () {

    }
}
