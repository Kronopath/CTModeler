using UnityEngine;
using System;
using System.Collections;

namespace CT {
    /// <summary>
    /// Random noise utility.
    /// </summary>
    public static class Noise {
        /// <summary>
        /// Gets smooth noise in 3D space.
        /// </summary>
        /// <returns>The value of the noise at the given location.</returns>
        /// <param name="P">The position in 3D space that you want to sample the noise values at.
        /// </param>
        public static float GetNoise(Vector3 P) {
            Func<Vector4, Vector4> floor = v => MapV4(v, Mathf.Floor);
            Func<Vector4, Vector4> mod289 = v => MapV4(v, x => x % 289.0f);
            Func<Vector4, Vector4> fract = v => MapV4(v, x => x - Mathf.Floor(x));
            Func<Vector4, Vector4> fade = v => MapV4(v, x => x * x * x * (x * (x * 6 - 15) + 10));

            Vector4 i0 = mod289(floor(P));
            Vector4 i1 = mod289(i0 + Vector4.one);
            Vector4 f0 = fract(P);
            Vector4 f1 = f0 - Vector4.one;
            Vector4 f = fade(f0);

            Vector4 ix = new Vector4(i0.x, i1.x, i0.x, i1.x);
            Vector4 iy = new Vector4(i0.y, i0.y, i1.y, i1.y);
            Vector4 iz0 = new Vector4(i0.z, i0.z, i0.z, i0.z);
            Vector4 iz1 = new Vector4(i1.z, i1.z, i1.z, i1.z);

            Func<Vector4, Vector4> permute = v => mod289(MapV4(v, x => (x * 34 + 1) * x));

            Vector4 ixy = permute(ix) + iy;
            Vector4 ixy0 = ixy + iz0;
            Vector4 ixy1 = ixy + iz1;

            Func<Vector4, Vector4> abs = v => MapV4(v, Mathf.Abs);
            Vector4 HALF4 = Vector4.one * 0.5f;

            Vector4 gx0 = ixy0 / 7f;
            Vector4 gx1 = ixy1 / 7f;
            Vector4 gy0 = fract(floor(gx0) / 7f) - HALF4;
            Vector4 gy1 = fract(floor(gx1) / 7f) - HALF4;
            gx0 = fract(gx0);
            gx1 = fract(gx1);
            Vector4 gz0 = HALF4 - abs(gx0) - abs(gy0);
            Vector4 gz1 = HALF4 - abs(gx1) - abs(gy1);

            Func<Vector4, Vector4> gt0 = v => MapV4(v, x => x > 0 ? 1 : 0);
            Func<Vector4, Vector4> lt0 = v => MapV4(v, x => x < 0 ? 1 : 0);

            Vector4 sz0 = gt0(gz0);
            Vector4 sz1 = gt0(gz1);

            gx0 = gx0 - Vector4.Scale(sz0, (lt0(gx0) - HALF4));
            gy0 = gy0 - Vector4.Scale(sz0, (lt0(gy0) - HALF4));
            gx1 = gx1 - Vector4.Scale(sz1, (lt0(gx1) - HALF4));
            gy1 = gy1 - Vector4.Scale(sz1, (lt0(gy1) - HALF4));

            Vector3 g0 = new Vector3(gx0.x, gy0.x, gz0.x);
            Vector3 g1 = new Vector3(gx0.y, gy0.y, gz0.y);
            Vector3 g2 = new Vector3(gx0.z, gy0.z, gz0.z);
            Vector3 g3 = new Vector3(gx0.w, gy0.w, gz0.w);
            Vector3 g4 = new Vector3(gx1.x, gy1.x, gz1.x);
            Vector3 g5 = new Vector3(gx1.y, gy1.y, gz1.y);
            Vector3 g6 = new Vector3(gx1.z, gy1.z, gz1.z);
            Vector3 g7 = new Vector3(gx1.w, gy1.w, gz1.w);

            Vector4 norm0 = new Vector4(
                                g0.magnitude,
                                g1.magnitude,
                                g2.magnitude,
                                g3.magnitude);
            Vector4 norm1 = new Vector4(
                                g4.magnitude,
                                g5.magnitude,
                                g6.magnitude,
                                g7.magnitude);

            g0 *= norm0.x;
            g1 *= norm0.y;
            g2 *= norm0.z;
            g3 *= norm0.w;

            g4 *= norm1.x;
            g5 *= norm1.y;
            g6 *= norm1.z;
            g7 *= norm1.w;

            Vector4 nz = Vector4.Lerp(
                             new Vector4(
                                 g0.x * f0.x + g0.y * f0.y + g0.z * f0.z,
                                 g1.x * f1.x + g1.y * f0.y + g2.z * f0.z,
                                 g2.x * f0.x + g2.y * f1.y + g2.z * f0.z,
                                 g3.x * f1.x + g3.y * f1.y + g3.z * f0.z),
                             new Vector4(
                                 g4.x * f0.x + g4.y * f0.y + g4.z * f1.z,
                                 g5.x * f1.x + g5.y * f0.y + g5.z * f1.z,
                                 g6.x * f0.x + g6.y * f1.y + g6.z * f1.z,
                                 g7.x * f1.x + g7.y * f1.y + g7.z * f1.z),
                             f.z);

            return 2.2f * Mathf.Lerp(
                Mathf.Lerp(nz.x, nz.z, f.y), Mathf.Lerp(nz.y, nz.w, f.y), f.x);
        }

        private static Vector4 MapV4(Vector4 input, Func<float, float> transformation) {
            return new Vector4(
                transformation(input.x),
                transformation(input.y),
                transformation(input.z),
                transformation(input.w));
        }

        private static Vector3 MapV3(Vector3 input, Func<float, float> transformation) {
            return new Vector3(
                transformation(input.x),
                transformation(input.y),
                transformation(input.z));
        }
    }
}