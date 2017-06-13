using UnityEngine;

public interface IGameMessage {
    ISquadModel Squad { get; }
}

public class PathAssignedMessage : IGameMessage {
    
    public ISquadModel Squad { get; private set; }
    public Path Path { get; private set; }
    
    public PathAssignedMessage (ISquadModel squad, Path path) {
        Squad = squad;
        Path = path;
    }
}

public class FormationChangedMessage : IGameMessage {
    
    public ISquadModel Squad { get; private set; }
    public string Formation { get; private set; }
    
    public FormationChangedMessage (ISquadModel squad, string formation) {
        Squad = squad;
        Formation = formation;
    }
}

public class SkillUsedMessage : IGameMessage {
    
    public ISquadModel Squad { get; private set; }
    public string Skill { get; private set; }
    
    public SkillUsedMessage (ISquadModel squad, string skill) {
        Squad = squad;
        Skill = skill;
    }
}

public class SquadSyncMessage : IGameMessage {
    
    public ISquadModel Squad { get; private set; }
    
    public int Side { get { return Squad.Side; } }
    public string Name { get { return Squad.Name; } }
    public float Health { get { return Squad.Health; } }
    public float Damage { get { return Squad.Damage; } }
    public long LastAttack { get { return Squad.LastAttack; } }
    public Vector2 Postion { get { return Squad.Postion; } }
    public Quaternion Rotation { get { return Squad.Rotation; } }
    public Path Path { get { return Squad.Path; } }
    public Vector2 LocalAim { get { return Squad.LocalAim; } }
    public Speed Speed { get { return Squad.Speed; } }
    public Vector2 Bounds { get { return Squad.Bounds; } }
    public string Formation { get { return Squad.Formation; } }
    public string OffensiveSkill { get { return Squad.OffensiveSkill; } }
    public string DefensiveSkill { get { return Squad.DefensiveSkill; } }
    
    public SquadSyncMessage (ISquadModel squad) {
        Squad = squad;
    }
}