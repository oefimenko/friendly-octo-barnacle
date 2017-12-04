using UnityEngine;

public interface IGameMessage {
	long Timestamp { get; }
    string SquadName { get; }
}

public class GameMessage : IGameMessage {

	private long timestamp;
	public long Timestamp { get { return timestamp; } }
	public string SquadName { get; set; }

	public GameMessage(long stamp) {
		timestamp = stamp;
	}

	public GameMessage() {
		timestamp = GameTime.Instance.Time();
	}
}

public class Connection : GameMessage {
	public new string SquadName { get; set; }

	public Connection (string username) : base() { 
		SquadName = username;
	}

	public Connection (long stamp, string username) : base(stamp) { 
		SquadName = username;
	}
}

public class InitMessage : GameMessage {

	public static int id = 12;

	public new string SquadName { get; set; }
	public int Port { get; set; }
	public string User1 { get; set; }
	public string User2 { get; set; }
	public int User1Side { get; set; }
	public int User2Side { get; set; }
	public SquadStruct[] Squads { get; set; }

	public InitMessage () : base() {
		SquadName = null;
	}

	public InitMessage (long stamp) : base(stamp) {
		SquadName = null;
	}
}

public class PathAssignedMessage : GameMessage {
    
	public static int id = 22;

	public new string SquadName { get; private set; }
    public IPath Path { get; private set; }
	public int NewVersion { get; private set; }
    
	public PathAssignedMessage (ISquadModel squad, IPath newPath) : base() {
		SquadName = squad.Name;
		Path = newPath;
		NewVersion = squad.Version + 1;
	}

	public PathAssignedMessage (long stamp, string squad, IPath path) : base(stamp) {
		SquadName = squad;
        Path = path;
    }
}

public class FormationChangedMessage : GameMessage {
    
	public static int id = 23;

	public new string SquadName { get; private set; }
    public string Formation { get; private set; }
	public int NewVersion { get; private set; }

	public FormationChangedMessage (ISquadModel squad, string formation) : base() {
		SquadName = squad.Name;
		Formation = formation;
		NewVersion = squad.Version + 1;
	}

	public FormationChangedMessage (long stamp, string squad, string formation) : base(stamp) {
		SquadName = squad;
        Formation = formation;
    }
}

public class SkillUsedMessage : GameMessage {
    
	public static int id = 24;

	public new string SquadName { get; private set; }
    public string Skill { get; private set; }
	public int NewVersion { get; private set; }

	public SkillUsedMessage (ISquadModel squad, string skill) : base() {
		SquadName = squad.Name;
		Skill = skill;
		NewVersion = squad.Version + 1;
	}

	public SkillUsedMessage (long stamp, string squad, string skill) : base(stamp) {
		SquadName = squad;
        Skill = skill;
    }
}

public class SquadSyncMessage : GameMessage {
    
	public static int id = 21;

	public new string SquadName { get; private set; }
    
//    public int Side { get { return Squad.Side; } }
//    public string Name { get { return Squad.Name; } }
//    public float Health { get { return Squad.Health; } }
//    public float Damage { get { return Squad.Damage; } }
//    public long LastAttack { get { return Squad.LastAttack; } }
//    public Vector2 Postion { get { return Squad.Postion; } }
//    public Quaternion Rotation { get { return Squad.Rotation; } }
//    public Path Path { get { return Squad.Path; } }
//    public Vector2 LocalAim { get { return Squad.LocalAim; } }
//    public Speed Speed { get { return Squad.Speed; } }
//    public Vector2 Bounds { get { return Squad.Bounds; } }
//    public string Formation { get { return Squad.Formation; } }
//    public string OffensiveSkill { get { return Squad.OffensiveSkill; } }
//    public string DefensiveSkill { get { return Squad.DefensiveSkill; } }
    
	public SquadSyncMessage (string squad) : base() {
		SquadName = squad;
	}

	public SquadSyncMessage (long stamp, string squad) : base(stamp) {
		SquadName = squad;
    }
}