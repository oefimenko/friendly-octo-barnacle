using UnityEngine;

public static class Utils {

	public static Vector3 Vector2To3 (Vector2 vector) {
		return new Vector3(vector.x, vector.y, 0f);
	}
	
	public static Vector2 Vector3To2 (Vector3 vector) {
		return new Vector2(vector.x, vector.y);
	}

	public static Quaternion SafeQuaternion (Vector3 frw, Vector3 up) {
		Quaternion targetRotation;
		if (frw == Vector3.zero && up == Vector3.zero) {
			targetRotation = Quaternion.identity;
		}
		else {
			targetRotation = Quaternion.LookRotation(frw, up);
		}
		return targetRotation;
	}
}
