using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnPathSetHandler (Path path, Speed speed);

public interface ISquadModel {

    event OnPathSetHandler OnPathSet;
    event OnObjectDestroy OnDestroy;

    int Side { get;  }
    string Name { get;  }
    float Health { get; set; }
    float Damage { get; }
    long LastAttack { get; set; }
    Vector2 Postion { get; set; }
    Path Path { get; set; }
    Vector2 Size { get; }

    string Formation { get; set; }
    Dictionary<string, Formation> Formations { get; }
    string OffensiveSkill { get; }
    string DefensiveSkill { get; }

    void Destroy (); 
}

public abstract class SquadModel : ISquadModel {

    protected int side;
    protected string name;
    protected string unitType;
    protected int unitCount;
    protected float maxHealth;
    protected float health;
    protected float damage;
    protected float attackCoolDown;
    protected long lastAttack;
    protected Speed speed;
    protected Vector2 position;
    protected Path path;
    protected Dictionary<IUnitModel, Vector2> units;
    protected Vector2 size;

    protected string formation;
    protected Dictionary<string, Formation> formations;
    protected string offensiveSkill;
    protected string defensiveSkill;

    public SquadModel (string iName, int iSide, Vector2 iPosition) {
        name = iName;
        side = iSide;
        health = maxHealth;
        lastAttack = -1;
        position = iPosition;
        units = new Dictionary<IUnitModel, Vector2>();
    }

    public event OnPathSetHandler OnPathSet = (Path path, Speed speed) => { };
    public event OnObjectDestroy OnDestroy = () => { };

    public int Side {
        get { return side; }
    }

    public string Name {
        get { return name; }
    }

    public float Health {
        get { return health; }
        set {
            // Change amount of units and attack if needed
            health = value;
        }
    }

    public float Damage {
        get { return damage; }
    }

    public long LastAttack {
        get { return lastAttack; }
        set { lastAttack = value; }
    }

    public Vector2 Postion {
        get { return position; }
        set { position = value; }
    }

    public Path Path {
        get { return path; }
        set {
            OnPathSet(value, speed);
            //Vector3 direction = (value - position).normalized;
            //Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.forward);
            //targetRotation.x = 0.0f;
            //targetRotation.y = 0.0f;
            //float angle = Quaternion.RotateTowards(Quaternion.identity, targetRotation, 1000000).eulerAngles.z;
            //int corr = (90 <= angle && angle >= 270) ? 1 : -1;
            //foreach (KeyValuePair<IUnitModel, Vector2> pair in units) {
            //    Vector3 offset = Quaternion.Euler(0, 0, angle) * pair.Value * corr;
            //    pair.Key.Aim = value + new Vector2(offset.x, offset.y);
            //}
            path = value;
        }
    }

    public Dictionary<IUnitModel, Vector2> Units {
        get { return units; }
    }

    public Vector2 Size {
        get { return size; }
    }

    public string Formation {
        get { return formation; }
        set {
            formation = value;
            Vector2 unitSize = UnitFactory.UnitSize(unitType);
            Vector2[] offset;
            formations[formation].Calculate(unitCount, unitSize, out offset, out size);
            int i = 0;
            foreach (KeyValuePair<IUnitModel, Vector2> pair in units) {
                pair.Key.Aim = position + new Vector2(offset[i].x, offset[i].y);
                i++;
            }
        }
    }

    public Dictionary<string, Formation> Formations {
        get { return formations; }
    }

    public string OffensiveSkill {
        get { return offensiveSkill; }
    }

    public string DefensiveSkill {
        get { return defensiveSkill; }
    }

    public void Destroy () {
        OnDestroy();
        foreach (KeyValuePair<IUnitModel, Vector2> pair in units) {
            pair.Key.Destroy();
        }
        foreach (KeyValuePair<string, Formation> formation in formations) {
            formation.Value.Destroy();
        }
        speed = null;
        path = null;
        units.Clear();
        formations.Clear();
    }

    protected void GenerateUnits () {
        Vector2 unitSize = UnitFactory.UnitSize(unitType);
        Vector2[] offset;
        formations[formation].Calculate(unitCount, unitSize, out offset, out size);
        for (int i = 0; i < offset.Length; i++) {
            GenerateUnit(i, offset[i]);
        }        
    }

    protected void GenerateUnit (int unitId, Vector2 offset) {
        string unitName = unitId + "(" + name + ")";
        Vector2 unitPosition = position + offset;
        IUnitModel unit = UnitFactory.Create(unitType, speed, unitName, unitPosition);
        units[unit] = offset;
    }

}
