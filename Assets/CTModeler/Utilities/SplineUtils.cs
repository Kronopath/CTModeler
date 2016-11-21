using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CT {
    public static class SplineUtils {
        public static List<Vector3> MakeSpline(List<Vector3> keys) {
            List<Vector3> spline = new List<Vector3>();

            int segmentsPerKey = 10;
            if(keys.Count < 2) {
                return new List<Vector3>(keys);
            }
            else if(keys.Count == 2) {
                for(int i = 0; i <= segmentsPerKey; i++) {
                    spline.Add(Vector3.Lerp(keys[0], keys[1], (float)i / segmentsPerKey));
                }
            }

            // TODO: actual splining
            return spline;
        }

        // TODO DOC THIS
        // t should be between 0 and 1!
        public static Vector3 Sample(List<Vector3> points, float t) {
            t = Mathf.Clamp(t, 0, 0.999f);
            if(points.Count < 1) {
                return Vector3.zero;
            }
            else if(points.Count == 1) {
                return points[0];
            }

            int intPart = Mathf.FloorToInt((points.Count - 1) * t);
            float fractPart = (points.Count - 1) * t - intPart;
            return Vector3.Lerp(points[intPart], points[intPart + 1], fractPart);
        }
    }
}