using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CT {
    public class Cup : Revolved {
        private List<Vector3> spline;

        public override void Start() {
            spline = SplineUtils.MakeSpline(
                new List<Vector3>{
                    new Vector3(1, 0.2f), new Vector3(0.2f, 1)
                }
            );
            base.Start();
        }

        protected override Vector2 RevolutionFunction(float t) {
            return SplineUtils.Sample(spline, t);
        }
    }
}
