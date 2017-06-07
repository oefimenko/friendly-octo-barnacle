using System.Collections.Generic;
using UnityEngine;

public delegate void OnPathSetHandler ();
public delegate void OnBoudsSetHandler (Vector2 size);

public interface ISquadModel {

    event OnPathSetHandler OnPathSet;
    event OnBoudsSetHandler OnBoundsSet;
    event OnObjectDestroy OnDestroy;

    int Side { get;  }
    string Name { get;  }
    float Health { get; set; }
    float Damage { get; }
    long LastAttack { get; set; }
    Vector2 Postion { get; set; }
    Quaternion Rotation { get; set; }
    Path Path { get; set; }
    Vector2 LocalAim { get; set; }
    Speed Speed { get; }
    Vector2 Bounds { get; }

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
    protected Quaternion rotation;
    protected Path path;
    protected Vector2 localAim;
    protected List<IUnitModel> units;
    protected Vector2 bounds;

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
        units = new List<IUnitModel>();
        rotation = Quaternion.identity;
    }

    public event OnPathSetHandler OnPathSet = () => { };
    public event OnObjectDestroy OnDestroy = () => { };
    public event OnBoudsSetHandler OnBoundsSet = (Vector2 bounds) => { };
    
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

    public Quaternion Rotation {
        get { return rotation; }
        set { rotation = value; }
    }
    
    public Path Path {
        get { return path; }
        set {
            if (path != null) Path.Destroy();
            path = value;
            if (path != null) OnPathSet();
        }
    }

    public Vector2 LocalAim {
        get { return localAim; }
        set {
            localAim = value;
            BattleArray bArray = formations[formation].Calculate(unitCount, position, rotation, localAim);
            Vector2[][] matrix = bArray.matrix;
            int k = 0;
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    units[k].Aim = matrix[i][j];
                    k++;
                }
            }
            Bounds = bArray.bounds;
        }
    }

    public Speed Speed {
        get { return speed;  }
    }

    public List<IUnitModel> Units {
        get { return units; }
    }

    public Vector2 Bounds {
        get { return bounds; }
        private set {
            bounds = value;
            OnBoundsSet(bounds);
        }

    }

    public string Formation {
        get { return formation; }
        set {
            formation = value;
            LocalAim = position;
//            BattleArray bArray = formations[formation].Calculate(unitCount, position, rotation, position);
//            Vector2[][] matrix = bArray.matrix;
//            int k = 0;
//            for (int i = 0; i < matrix.Length; i++)
//            {
//                for (int j = 0; j < matrix[i].Length; j++)
//                {
//                    units[k].Aim = matrix[i][j];
//                    k++;
//                }
//            }
//            Bounds = bArray.bounds;
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
        for (int i = 0; i < units.Count; i++) {
            units[i].Destroy();
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
        BattleArray bArray = formations[formation].Calculate(unitCount, position, rotation, position);
        Vector2[][] matrix = bArray.matrix;
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                string unitName = i.ToString() + j.ToString() + "(" + name + ")";
                IUnitModel unit = UnitFactory.Create(unitType, speed, unitName, matrix[i][j]);
                units.Add(unit);
            }
        }
        bounds = bArray.bounds;
    }
}
