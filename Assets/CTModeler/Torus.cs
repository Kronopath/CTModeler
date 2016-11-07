using UnityEngine;
using System.Collections;

namespace CT {
    public class Torus : Revolved {

        public float radius = 0.3f;

        protected override Vector2 RevolutionFunction(float t) {
            float phi = 2 * Mathf.PI * t;
            return new Vector2(1 - radius * Mathf.Cos(phi), -radius * Mathf.Sin(phi));
        }
    }
}