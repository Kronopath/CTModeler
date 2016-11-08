using UnityEngine;
using System.Collections;

namespace CT {
    public class OpenCylinder : Revolved {
        protected override Vector2 RevolutionFunction(float t) {
            return new Vector2(1, 2 * t - 1);
        }
    }
}