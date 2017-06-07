using System.Collections.Generic;
using UnityEngine;

public class SkeletonModel : SquadModel {

	public SkeletonModel (string iName, int iSide, Vector2 iPosition) : base(iName, iSide, iPosition) {
        unitType = "Skeleton";
        unitCount = 20;
        maxHealth = 300f;
        damage = 25f;
        attackCoolDown = 1f;
        speed = new Speed(2f, 450f);
        formations = new Dictionary<string, Formation>() {
            { "Rectangle", new Rectangle(UnitFactory.UnitSize(unitType)) },
            { "Inline", new Inline(UnitFactory.UnitSize(unitType)) }
        };
        formation = "Rectangle";
        offensiveSkill = "Fast attack";
        defensiveSkill = "Retreat";
        GenerateUnits();
    }
}
