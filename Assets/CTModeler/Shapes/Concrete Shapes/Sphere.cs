using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// Generates a full or partial sphere.
    /// </summary>
    public class Sphere : Revolved {

        /// <summary>
        /// Increase this to cut away part of the sphere starting at one of the poles, allowing you
        /// to have domes, half-spheres, and so on.
        /// </summary>
        [Range(0.0f, 1.0f)]
        public float amountToCutOff = 0.0f;

        protected override Vector2 RevolutionFunction(float t) {
            float phi = Mathf.PI * (Mathf.Max(amountToCutOff, t) - 0.5f);
            return new Vector2(Mathf.Cos(phi), Mathf.Sin(phi));
        }
    }
}