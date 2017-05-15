using UnityEngine;

public class SplineBounds {

    private const int BaseApp = 9;
    private Vector3 baseStart;
    private Vector3 baseFinish;
    private Vector3 startCP;
    private Vector3 finishCP;

    public SplineBounds (Vector3[] arr, Vector3 initialDirect) {
        baseStart = new Vector3(arr[0].x, arr[0].y, 0);
        baseFinish = new Vector3(arr[2].x, arr[2].y, 0);
        float power = Vector3.Distance(baseStart, baseFinish) * 0.35f;
        startCP = baseStart + initialDirect.normalized * power;
        finishCP = baseFinish + (startCP - baseFinish).normalized * power;
    }

    public OrientedPoint[] GetPoints (int approx = BaseApp) {
        OrientedPoint[] result = new OrientedPoint[approx];
        float t = 0.01f;
        float step = 1f / approx;
        for (int i = 0; i < approx; i++) {
            result[i] = new OrientedPoint(GetPoint(t), GetOrientation(t));
            t += step;
        }
        return result;
    }

    private Vector3 GetPoint (float t) {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        return baseStart * (omt2 * omt) +
                startCP * (3f * omt2 * t) +
                finishCP * (3f * omt * t2) +
                baseFinish * (t2 * t);
    }

    private Quaternion GetOrientation (float t) {
        Vector3 tng = GetTangent(t);
        Vector3 nrm = GetNormal2D(tng);
        Vector3 frw = Vector3.Cross(tng, nrm);
        Quaternion result;
        if (tng == Vector3.zero && frw == Vector3.zero) {
            result = Quaternion.identity;
        } else {
            result = Quaternion.LookRotation(frw, nrm);
        }
        return result;
    }

    private Vector3 GetTangent (float t) {
        float omt = 1f - t;
        float omt2 = omt * omt;
        float t2 = t * t;
        Vector3 tangent =
                    baseStart * (-omt2) +
                    startCP * (3 * omt2 - 2 * omt) +
                    finishCP * (-3 * t2 + 2 * t) +
                    baseFinish * (t2);
        return tangent.normalized;
    }

    private Vector3 GetNormal2D (Vector3 tng) {
        return new Vector3(-tng.y, tng.x, 0f);
    }

}
