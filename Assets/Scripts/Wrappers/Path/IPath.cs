using UnityEngine;

public interface IPath {

	void Update (Vector3 position);

	void Complete (Vector3 position);

	Vector2? NextPoint ();

	void Destroy ();

	string ToString ();

}
