using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CT {
    public static class SplineUtils {
        // Unity's version of Mono doesn't accept more than 4 arguments in a Func<T...>, sadly, so
        // this is necessary.
        private delegate Vector3 HermiteFunc (Vector3 a,Vector3 da,Vector3 b,Vector3 db,float t);

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

            HermiteFunc hermite =
                (Vector3 a, Vector3 da, Vector3 b, Vector3 db, float t) => {
                    float tt = t * t;
                    float ttt = tt * t;
                    return a * (2 * ttt - 3 * tt + 1)
                    + da * (ttt - 2 * tt + t)
                    + b * (-2 * ttt + 3 * tt)
                    + db * (ttt - tt);
                };

            List<float> lengths = new List<float>();
            for(int n = 0; n < keys.Count - 1; n++) {
                lengths.Add((keys[n + 1] - keys[n]).magnitude);
            }

            List<Vector3> D = new List<Vector3>();
            for(int n = 0; n < keys.Count; n++) {
                if(n == 0) {
                    D.Add((3 * keys[n + 1] - 2 * keys[n] - keys[n + 2]) / 3);
                }
                else if(n < keys.Count - 1) {
                    D.Add(
                        (lengths[n] * (keys[n] - keys[n - 1])
                        + lengths[n - 1] * (keys[n + 1] - keys[n]))
                        / (lengths[n - 1] + lengths[n]));
                }
                else {
                    D.Add((2 * keys[n] - 3 * keys[n - 1] + keys[n - 2]) / 3);
                }
            }

            if(keys[0] == keys[keys.Count - 1]) {
                Vector3 average = (D[0] + D[keys.Count - 1]) / 2;
                D[0] = average;
                D[keys.Count - 1] = average;
            }

            for(int n = 0; n < keys.Count - 1; n++) {
                for(int i = 0; i < segmentsPerKey; i++) {
                    spline.Add(hermite(keys[n], D[n] * 0.9f, keys[n + 1], D[n + 1] * 0.9f,
                                       i / segmentsPerKey));
                }
            }
            spline.Add(keys[keys.Count - 1]);

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