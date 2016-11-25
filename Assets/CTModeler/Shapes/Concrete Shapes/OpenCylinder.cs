using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// A cylinder shape that's open at both ends (i.e. uncapped).
    /// </summary>
    public class OpenCylinder : Revolved {

        [Label("<b>NOTE:</b> Don't change Num U and Num V above.\n" +
               "This object will automatically set them to\n" +
               "appropriate values based on Num Segments.")]
        /// <summary>
        /// The number of segments around the circle used to make this cylinder.
        /// </summary>
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