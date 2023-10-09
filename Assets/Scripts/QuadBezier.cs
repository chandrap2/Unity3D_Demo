using UnityEngine;

public class QuadBezier : Bezier {

    private Vector3 p1, p2, p3;
    private Vector3 quadTerm, linearTerm;

    public QuadBezier(Vector3 p1, Vector3 p2, Vector3 p3) {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;

        ComputeCoeffs();
        ComputeEndpoints();
    }

    protected override void ComputeCoeffs() {
        quadTerm = p1 - 2 * p2 + p3;
        linearTerm = -2 * p1 + 2 * p2;
    }

    protected override Vector3 ReturnPos(float t) {
        Vector3 result;

        float tSqr = t * t;

        result = quadTerm * tSqr;
        result += linearTerm * t;
        result += p1;

        return result;
    }
}
