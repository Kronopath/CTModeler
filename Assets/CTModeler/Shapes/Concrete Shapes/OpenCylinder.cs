using UnityEngine;
using System.Collections;

namespace CT {
    public class OpenCylinder : Revolved {

        // TODO: write custom editor that only shows num segments and hides numU and numV
        public int numSegments = 10;

        public override void Start() {
            numU = numSegments;
            numV = 1;
            base.Start();
        }

        protected override Vector2 RevolutionFunction(float t) {
            return new Vector2(1, 2 * t - 1);
        }
    }
}