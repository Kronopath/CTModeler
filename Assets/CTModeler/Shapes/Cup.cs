using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CT {
    public class Cup : Revolved {
        private List<Vector3> spline;

        public override void Start() {
            spline = SplineUtils.MakeSpline(
                new List<Vector3> {
                    new Vector3(.001f, -1f),
                    new Vector3(.5f, -1f),
                    new Vector3(.45f, -.95f),
                    new Vector3(.15f, -.9f),
                    new Vector3(.09f, -.85f),
                    new Vector3(.05f, -.6f),
                    new Vector3(.1f, 0f),
                    new Vector3(.2f, .1f),
                    new Vector3(.4f, .2f),
                    new Vector3(.6f, .5f),
                    new Vector3(.5f, .98f),
                    new Vector3(.48f, 1f),
                    new Vector3(.46f, .98f),
                    new Vector3(.55f, .5f),
                    new Vector3(.35f, .3f),
                    new Vector3(.001f, .2f)
                }
            );
            base.Start();
        }

        protected override Vector2 RevolutionFunction(float t) {
            return SplineUtils.Sample(spline, t);
        }
    }
}
