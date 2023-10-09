using UnityEngine;

public class CubicBezier : Bezier
{

    private Vector3 p1, p2, p3, p4;
    private Vector3 cubicTerm, quadTerm, linearTerm;

    public CubicBezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
        this.p4 = p4;

        ComputeCoeffs();
        ComputeEndpoints();
    }

    protected override void ComputeCoeffs() {
        cubicTerm = -p1 + 3 * p2 - 3 * p3 + p4;
        quadTerm = 3 * p1 - 6 * p2 + 3 * p3;
        linearTerm = -3 * p1 + 3 * p2;
    }

    protected override Vector3 ReturnPos(float t) {
        Vector3 result;

        float tSqr = t * t;
        float tCube = tSqr * t;

        result = cubicTerm * tCube;
        result += quadTerm * tSqr;
        result += linearTerm * t;
        result += p1;

        return result;
    }
}
