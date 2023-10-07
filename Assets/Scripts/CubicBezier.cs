using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CubicBezier
{
    public enum TRAVEL_STATE {
        HIT_START, TRAVELLED, HIT_END
    };

    private const int NUM_SEGMENTS = 200;

    private Vector3 p1, p2, p3, p4;
    private Vector3 cubicTerm, quadTerm, linearTerm;
    
    private Vector3[] segmentEndpoints;
    private float[] cumulArcLength;
    private float dt;

    private int currSegment;
    private float currDist;

    public CubicBezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
        this.p1 = p1;
        this.p2 = p2;
        this.p3 = p3;
        this.p4 = p4;

        segmentEndpoints = new Vector3[NUM_SEGMENTS + 1];
        cumulArcLength = new float[NUM_SEGMENTS + 1];
        dt = 1f / NUM_SEGMENTS;

        currSegment = 0;
        currDist = 0;

        ComputeCoeffs();
        ComputeEndpoints();
    }

    private void ComputeEndpoints() {
        float currT = 0;
        for (int i = 0; i < NUM_SEGMENTS + 1; i++) {
            segmentEndpoints[i] = ReturnPos(currT);

            if (i == 0) {
                cumulArcLength[0] = 0;
            } else {
                cumulArcLength[i] = cumulArcLength[i - 1] +
                    Vector3.Magnitude(segmentEndpoints[i] - segmentEndpoints[i - 1]);
            }

            currT += dt;
        }
    }

    private void ComputeCoeffs() {
        cubicTerm = -p1 + 3 * p2 - 3 * p3 + p4;
        quadTerm = 3 * p1 - 6 * p2 + 3 * p3;
        linearTerm = -3 * p1 + 3 * p2;
    }

    public Vector3 ReturnPos(float t) {
        Vector3 result;

        float tSqr = t * t;
        float tCube = tSqr * t;

        result = cubicTerm * tCube;
        result += quadTerm * tSqr;
        result += linearTerm * t;
        result += p1;

        return result;
    }

    public Vector3 MoveAlongCurve(float dist, Vector3 posToChange) {
        float targetDist = dist + currDist;

        int segmentIndex;
        if (dist > 0) {
            segmentIndex = ~Array.BinarySearch(cumulArcLength, currSegment, cumulArcLength.Length - currSegment, targetDist) - 1;
        } else {
            segmentIndex = ~Array.BinarySearch(cumulArcLength, 0, currSegment + 1, targetDist) - 1;
        }

        segmentIndex = Math.Clamp(segmentIndex, 0, NUM_SEGMENTS);

        Vector3 finalPos;
        if (segmentIndex == 0 || segmentIndex == NUM_SEGMENTS) {
            finalPos = segmentEndpoints[segmentIndex];
            currDist = cumulArcLength[segmentIndex];
        } else {
            float tAlongSegment = (targetDist - cumulArcLength[segmentIndex]) / (cumulArcLength[segmentIndex + 1] - cumulArcLength[segmentIndex]);
            finalPos = (segmentEndpoints[segmentIndex] * (1 - tAlongSegment)) + (segmentEndpoints[segmentIndex + 1] * tAlongSegment);
            currDist = targetDist;
        }

        currSegment = segmentIndex;
        //posToChange = finalPos;
        return finalPos;
    }
}
