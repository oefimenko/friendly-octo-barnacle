using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherModel : SquadModel {

	public SkeletonArcherModel (string iName, string iOwner, Vector2 iPosition) : base(iName, iOwner, iPosition) {
		unitType = "SkeletonArcher";
		unitCount = 20;
		maxHealth = 300f;
		damage = 25f;
		attackCoolDown = 1f;
		speed = new Speed(2f, 450f);
		formations = new Dictionary<string, Formation>() {
			{ "Rectangle", new Rectangle(UnitFactory.UnitSize(unitType)) },
			{ "Inline", new Inline(UnitFactory.UnitSize(unitType)) },
			{ "Column", new Column(UnitFactory.UnitSize(unitType)) }
		};
		formation = "Rectangle";
		offensiveSkill = "Fast attack";
		defensiveSkill = "Retreat";
		GenerateUnits();
	}
}
