using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// Generates a torus, or donut shape.
    /// </summary>
    public class Torus : Revolved {
        /// <summary>
        /// The minor radius of the torus, i.e. the radius of the "tube".
        /// </summary>
        public float radius = 0.3f;

        protected override Vector2 RevolutionFunction(float t) {
            float phi = 2 * Mathf.PI * t;
            return new Vector2(1 - radius * Mathf.Cos(phi), -radius * Mathf.Sin(phi));
        }
    }
}