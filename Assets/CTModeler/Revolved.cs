using UnityEngine;
using System.Collections;

namespace CT {
    public abstract class Revolved : Parametric {

        protected abstract Vector2 RevolutionFunction(float t);

        protected sealed override Vector3 ParametricFunction(float u, float v) {
            float theta = 2 * Mathf.PI * -u;
            Vector2 xz = RevolutionFunction(v);
            return new Vector3(xz.x * Mathf.Cos(theta),
                               xz.x * Mathf.Sin(theta),
                               xz.y);
        }
    }
}