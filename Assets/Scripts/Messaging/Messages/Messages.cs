using UnityEngine;

public interface IGameMessage {
    string SquadName { get; }
}

public class Connection : IGameMessage {
	public string SquadName { get; set; }
	public Connection (string username) { 
		SquadName = username;
	}
}

public class InitMessage : IGameMessage {

	public static int id = 12;

	public string SquadName { get; set; }
	public int Port { get; set; }
	public string User1 { get; set; }
	public string User2 { get; set; }
	public int User1Side { get; set; }
	public int User2Side { get; set; }
	public SquadStruct[] Squads { get; set; }

	public InitMessage () {
		SquadName = null;
	}
}

public class PathAssignedMessage : IGameMessage {
    
	public static int id = 22;

	public string SquadName { get; private set; }
    public IPath Path { get; private set; }
    
	public PathAssignedMessage (string squad, IPath path) {
		SquadName = squad;
        Path = path;
    }
}

public class FormationChangedMessage : IGameMessage {
    
	public static int id = 23;

	public string SquadName { get; private set; }
    public string Formation { get; private set; }
    
	public FormationChangedMessage (string squad, string formation) {
		SquadName = squad;
        Formation = formation;
    }
}

public class SkillUsedMessage : IGameMessage {
    
	public static int id = 24;

	public string SquadName { get; private set; }
    public string Skill { get; private set; }
    
	public SkillUsedMessage (string squad, string skill) {
		SquadName = squad;
        Skill = skill;
    }
}

public class SquadSyncMessage : IGameMessage {
    
	public static int id = 21;

	public string SquadName { get; private set; }
    
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
    
	public SquadSyncMessage (string squad) {
		SquadName = squad;
    }
}