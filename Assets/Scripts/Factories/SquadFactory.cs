using UnityEngine;

public class SquadFactory {

    public static ISquadModel Create(string type, string name, int side, Vector2 position) {
        ISquadModel model = CreateSquadModel(type, name, side, position);
        GameObject prefab = ResourceManager.Instance.Get("Squads", type);
        GameObject gObject = Object.Instantiate(prefab, new Vector3(position.x, position.y), Quaternion.identity);
        SquadView view = gObject.AddComponent<SquadView>();
        new SquadController(model, view);
        gObject.name = name;
        SquadMonitor.Instance.Add(name, model);
        view.GetComponent<BoxCollider2D>().size = model.Bounds;
        return model;
    }

    private static ISquadModel CreateSquadModel (string type, string name, int side, Vector2 position) {
        ISquadModel model;

        switch (type)
        {
            case "SkeletonSquad":
                model = new SkeletonModel(name, side, position);
                break;
            case "SpiderSquad":
                model = new SpiderModel(name, side, position);
                break;
            default:
                model = null;
                break;
        }
        return model;
    }
}
