using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// A simple, flat, circular disk.
    /// </summary>
    public class Disk : Revolved {
        
        [Label("<b>NOTE:</b> Don't change Num U and Num V above.\n" +
               "This object will automatically set them to\n" +
               "appropriate values based on Num Segments.")]
        /// <summary>
        /// The number of triangles/wedges this disk should be built of.
        /// </summary>
        public int numSegments = 10;

        public override void Start() {
            base.Start();
        }

        protected override Vector2 RevolutionFunction(float t) {
            return new Vector2(t + 0.001f, 0);
        }

        public override void OnValidate() {
            numU = numSegments;
            numV = 1;
            base.OnValidate();
        }
    }
}