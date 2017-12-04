using UnityEngine;

public static class UnitFactory {

    public static IUnitModel Create(string type, Speed speed, string name, Vector2 position) {
        IUnitModel model = new UnitModel(speed);
        GameObject prefab = ResourceManager.Instance.Get("Units", type);
        GameObject gObject = Object.Instantiate(prefab, new Vector3(position.x, position.y), Quaternion.identity);
        IUnitView view = gObject.AddComponent<UnitView>();
        new UnitController(model, view);
        gObject.name = type + name;
        return model;
    }

    public static Vector2 UnitSize (string unitType) {
        GameObject unitPrefab = ResourceManager.Instance.Get("Units", unitType);
        Vector2 baseSize = unitPrefab.GetComponent<BoxCollider2D>().size;
//        Vector2 scale = unitPrefab.transform.localScale;
		return new Vector2(baseSize.x, baseSize.y);
//        return new Vector2(baseSize.x + scale.x, baseSize.y + scale.y);
    }
}
