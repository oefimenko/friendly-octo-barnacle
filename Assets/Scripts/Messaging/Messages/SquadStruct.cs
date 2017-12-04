using System;
using UnityEngine;

public class SquadStruct {

	public string Owner { get { return owner; } }
	public string Name { get { return name; } }
	public string Type { get { return type; } }
	public float Health { get { return health; } }
	public float Damage { get { return damage; } }
	public long LastAttack { get { return lastAttack; } }
	public Vector2 Postion { get { return postion; } }
	public Quaternion Rotation { get { return rotation; } }
	public IPath Path { get { return path; } }
	public Vector2 LocalAim { get { return localAim; } }
	public Speed Speed { get { return speed; } }
	public Vector2 Bounds { get { return bounds; } }
	public string Formation { get { return formation; } }
	public string OffensiveSkill { get { return offensiveSkill; } }
	public string DefensiveSkill { get { return defensiveSkill; } }

	private string owner;
	private string name;
	private string type;
	private float health;
	private float damage;
	private long lastAttack;
	private Vector2 postion;
	private Quaternion rotation;
	private IPath path;
	private Vector2 localAim;
	private Speed speed;
	private Vector2 bounds;
	private string formation;
	private string offensiveSkill;
	private string defensiveSkill;

	private SquadStruct () {}

	public static SquadStruct FromSquadModel (ISquadModel model) {
		SquadStruct squad = new SquadStruct ();
		squad.owner = model.Owner;
		squad.name = model.Name;
		//squad.type;
		squad.health = model.Health;
		squad.damage = model.Damage;
		squad.lastAttack = model.LastAttack;
		squad.postion = model.Postion;
		squad.rotation = model.Rotation;
		squad.path = model.Path;
		squad.localAim = model.LocalAim;
		squad.speed = model.Speed;
		squad.bounds = model.Bounds;
		squad.formation = model.Formation;
		squad.offensiveSkill = model.OffensiveSkill;
		squad.defensiveSkill = model.DefensiveSkill;

		return squad;
	}

	public static SquadStruct FromString (string pyl) {
		SquadStruct squad = new SquadStruct ();
		string[] prms = pyl.Split (':');

		squad.owner = prms[1];
		squad.name = prms[2];
		squad.type = prms[0];
//		squad.health = model.Health;
//		squad.damage = model.Damage;
//		squad.lastAttack = model.LastAttack;
		squad.postion = new Vector2(float.Parse(prms[6]) / 1000f, float.Parse(prms[7]) / 1000f);
//		squad.rotation = model.Rotation;
//		squad.path = model.Path;
//		squad.localAim = model.LocalAim;
//		squad.speed = model.Speed;
//		squad.bounds = model.Bounds;
//		squad.formation = model.Formation;
//		squad.offensiveSkill = model.OffensiveSkill;
//		squad.defensiveSkill = model.DefensiveSkill;
		return squad;
	}


}
