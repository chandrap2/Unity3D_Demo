using System;
using UnityEngine;

public abstract class Bezier
{
    public enum TRAVEL_STATE {
        HIT_START, TRAVELLED, HIT_END
    };

    protected const int NUM_SEGMENTS = 200;

    protected Vector3[] segmentEndpoints;
    protected float[] cumulArcLength;
    protected float dt;

    protected int currSegment;
    protected float currDist;

    public Bezier() {
        segmentEndpoints = new Vector3[NUM_SEGMENTS + 1];
        cumulArcLength = new float[NUM_SEGMENTS + 1];
        dt = 1f / NUM_SEGMENTS;

        currSegment = 0;
        currDist = 0;
    }

    protected abstract void ComputeCoeffs();
    protected abstract Vector3 ReturnPos(float t);

    protected void ComputeEndpoints() {
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

    public Vector3 MoveAlongCurve(float dist) {
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
        return finalPos;
    }
}
