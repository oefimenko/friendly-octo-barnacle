using UnityEngine;

public class UnitController {

    private IUnitModel model;
    private IUnitView view;

    public UnitController (IUnitModel nModel, IUnitView nView) {
        model = nModel;
        view = nView;
        // Subscriptions
        model.OnAimSet += AimChange;
        model.OnDestroy += Destroy;
        view.OnPositionChange += PositionChange;
        view.OnUnitStop += UnitStop;
    }

    public void Destroy () {
        model.OnAimSet -= AimChange;
        model.OnDestroy -= Destroy;
        view.OnPositionChange -= PositionChange;
        view.OnUnitStop -= UnitStop;
        view.Destroy();
        model = null;
        view = null;
    }

    private void AimChange (Vector2 newAim, Speed speed) {
        view.SetState(1);
        model.State = 1;
        view.Navigate(newAim, speed);
    }

    private void PositionChange (Vector2 newPosition) {
        model.Position = newPosition;
    }

    private void UnitStop () {
        view.SetState(0);
        model.State = 0;
    }
}
