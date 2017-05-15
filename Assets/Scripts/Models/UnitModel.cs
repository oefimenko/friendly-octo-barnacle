using UnityEngine;

public delegate void AimSetHandler(Vector2 aim, Speed speed);

public interface IUnitModel {

    event AimSetHandler OnAimSet;
    event OnObjectDestroy OnDestroy;

    int State { get; set; }
    Vector2 Position { get; set; }
    Vector2 Aim { get; set; }

    void Destroy ();
}

public class UnitModel : IUnitModel {

    // Events
    public event AimSetHandler OnAimSet = (Vector2 aim, Speed speed) => { };
    public event OnObjectDestroy OnDestroy = () => { };

    private Speed speed;
    private int state;
    private Vector2 position;
    private Vector2 aim;

	public UnitModel (Speed initialSpeed) {
        speed = initialSpeed;
    }

    public int State {
        get { return state; }
        set { state = value; }
    }

    public Vector2 Position {
        get { return position; }
        set { position = value; }
    }
    public Vector2 Aim {
        get { return aim; }
        set {
            OnAimSet(value, speed);
            aim = value;
        }
    }

    public void Destroy () {
        speed = null;
        OnDestroy();
    }
}
