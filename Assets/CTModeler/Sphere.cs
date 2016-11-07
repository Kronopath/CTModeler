using UnityEngine;
using System.Collections;

namespace CT {
    public class Sphere : Revolved {

        [Range(0.0f, 1.0f)]
        public float amountToCutOff;

        protected override Vector2 RevolutionFunction(float t) {
            float phi = Mathf.PI * (Mathf.Max(amountToCutOff, t) - 0.5f);
            return new Vector2(Mathf.Cos(phi), Mathf.Sin(phi));
        }
    }
}