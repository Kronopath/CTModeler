using UnityEngine;
using System.Collections;

namespace CT {
    public class OpenCylinder : Revolved {

        [Label("<b>NOTE:</b> Don't change Num U and Num V above.\n" +
               "This object will automatically set them to\n" +
               "appropriate values based on Num Segments.")]
        public int numSegments = 10;

        public override void Start() {
            base.Start();
        }

        protected override Vector2 RevolutionFunction(float t) {
            return new Vector2(1, 2 * t - 1);
        }

        public override void OnValidate() {
            numU = numSegments;
            numV = 1;
            base.OnValidate();
        }
    }
}