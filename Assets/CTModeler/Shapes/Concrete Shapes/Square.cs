using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// A simple flat 2x2 square.
    /// </summary>
    public class Square : Parametric {
        protected override Vector3 ParametricFunction(float u, float v) {
            return new Vector3(2 * u - 1, 2 * v - 1, 0);
        }
    }
}