﻿using UnityEngine;

public class OrientedPoint {

    private Vector3 position;
    private Quaternion rotation;

	public OrientedPoint (Vector3 position, Quaternion rotation) {
        this.position = position;
        this.rotation = rotation;
    }

    public Vector3 LocalToWorld (Vector3 point) {
        return position + rotation * point;
	}

	public Vector3 WorldToLocal (Vector3 point) {
		return Quaternion.Inverse(rotation) * ( point - position);
	}

	public Vector3 LocalToWorldDirection (Vector3 dir) {
		return rotation * dir;
	}
}