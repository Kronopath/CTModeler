using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// This implements support for a mesh defined by a surface of revolution around the Z axis.
    /// </summary>
    public abstract class Revolved : Parametric {

        /// <summary>
        /// Override this function to define the shape of the revolved surface.
        /// </summary>
        /// <returns>The point on the XY plane that will be revolved around the axis.</returns>
        /// <param name="t">The parameter, which varies from 0 to 1.</param>
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