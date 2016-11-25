using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CT {
    /// <summary>
    /// A utility for creating, and sampling from, spline curves.
    /// </summary>
    public static class SplineUtils {

        /// <summary>
        /// Given a list of control points, generates a smooth path that passes through them.
        /// </summary>
        /// <returns>An array of Vector3s that trace the path of the spline.</returns>
        /// <param name="keys">A list of key points that the spline should pass through in order.</param>
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
                    spline.Add(Hermite(keys[n], D[n] * 0.9f, keys[n + 1], D[n + 1] * 0.9f,
                                       (float)i / segmentsPerKey));
                }
            }
            spline.Add(keys[keys.Count - 1]);

            return spline;
        }

        /// <summary>
        /// Samples a value from a curve.
        /// </summary>
        /// <returns>Either the closest point from the input array, or a linear interpolation
        /// between the two closest points.</returns>
        /// <param name="points">A set of points defining a curve. The more points there are,
        /// the smoother the sampling will be. All points are assumed to be equal "distance" apart in
        /// the parametric axis t.</param>
        /// <param name="t">The parameter for how far along the curve we are.
        /// Must be between 0 and 1, with 0 being the beginning of the curve and 1 being the end.
        /// </param>
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

        private static Vector3 Hermite(Vector3 a, Vector3 da, Vector3 b, Vector3 db, float t) {
            float tt = t * t;
            float ttt = tt * t;
            return a * (2 * ttt - 3 * tt + 1)
            + da * (ttt - 2 * tt + t)
            + b * (-2 * ttt + 3 * tt)
            + db * (ttt - tt);
        }
    }
}