using System;

public class Deserializer {

	public Deserializer () {}

	public InitMessage ParseInit (string body) {
		
		InitMessage result = new InitMessage ();
		string[] prms = body.Split (';');

		SquadStruct[] squads = new SquadStruct[prms.Length];
		for (int i = 0; i < squads.Length; i++) {
			squads [i] = SquadStruct.FromString (prms [i]);
		}
		result.Squads = squads;
		return result;
	}

	public FormationChangedMessage ParseFormation (string body) {
		string[] prms = body.Split(';');
		string[] squad_prms = prms[1].Split(':');
		return new FormationChangedMessage (squad_prms[2], squad_prms[9]);
	}

	public PathAssignedMessage ParsePath (string body) {
		string[] prms = body.Split(';');
		string[] squad_prms = prms[1].Split(':');
		string name = squad_prms[18] + squad_prms[19] + squad_prms[squad_prms.Length - 2] + squad_prms[squad_prms.Length - 1];
		IPath path = PathsHandler.Instance.Get(name);

		if (path == null) {
			path = new SlientPath();
			for (int i = 18; i < squad_prms.Length - 2; i += 2) {
				path.Update(new UnityEngine.Vector3(
								Int16.Parse(squad_prms[i]) / 1000f, 
								Int16.Parse(squad_prms[i + 1]) / 1000f,
								0f
				));
			}
			path.Complete(new UnityEngine.Vector3(
				Int16.Parse(squad_prms[squad_prms.Length - 2]) / 1000f, 
				Int16.Parse(squad_prms[squad_prms.Length - 1]) / 1000f,
				0f
			));
		}

		return new PathAssignedMessage (squad_prms[2], path);
	}
}
