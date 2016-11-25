using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// A fancy-looking loopy extruded surface.
    /// </summary>
    public class Knot : Extruded {

        private const float TAU = 2 * Mathf.PI;

        protected override Vector3 ExtrusionPathFunction(float v) {
            v *= 2 * TAU;
            float r = 1 + Mathf.Cos(1.5f * v) / 5;
            return new Vector3(Mathf.Sin(1.5f * v) / 2,
                               r * Mathf.Cos(v),
                               r * Mathf.Sin(v));
        }

        protected override Vector2 ExtrusionProfileFunction(float u, float v) {
            v = 0.15f + 0.06f * Mathf.Cos(3 * TAU * v);
            u *= TAU;
            return new Vector2(v*Mathf.Cos(u), v*Mathf.Sin(u));
        }
    }
}