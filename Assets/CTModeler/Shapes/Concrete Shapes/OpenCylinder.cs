using UnityEngine;
using System.Collections;

namespace CT {
    public class OpenCylinder : Revolved {

        [Label("<b>NOTE:</b> Don't change Num U and Num V above.\n" +
        "This object will automatically set them to appropriate values based on Num Segments.")]
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