using System.Collections.Generic;
using UnityEngine;

public class SpiderModel : SquadModel {

	public SpiderModel (string iName, string iOwner, Vector2 iPosition) : base(iName, iOwner, iPosition) {
        unitType = "Spider";
        unitCount = 16;
        maxHealth = 300f;
        damage = 25f;
        attackCoolDown = 1f;
        speed = new Speed(2f, 450f);
        formations = new Dictionary<string, Formation>() {
            { "Rectangle 2", new Rectangle(UnitFactory.UnitSize(unitType)) },
            { "Inline 1", new Inline(UnitFactory.UnitSize(unitType)) }
        };
        formation = "Rectangle 2";
        offensiveSkill = "Poison attack";
        defensiveSkill = "Hide";
        GenerateUnits();
    }
}
