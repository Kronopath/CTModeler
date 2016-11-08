using UnityEngine;
using System.Collections;

namespace CT {
    public abstract class Extruded : Parametric {
        
        protected abstract Vector2 ExtrusionProfileFunction(float u, float v);

        protected abstract Vector3 ExtrusionPathFunction(float v);

        protected sealed override Vector3 ParametricFunction(float u, float v) {
            Vector2 xy = ExtrusionProfileFunction(u, v);
            Vector3 path = ExtrusionPathFunction(v);
            Vector3 path2 = ExtrusionPathFunction(v + 0.001f);
            Vector3 dz = (path2 - path).normalized;
            Vector3 dx = Vector3.right;
            Vector3 dy = Vector3.Cross(dx, dz).normalized;
            return new Vector3(path.x + xy.x * dx.x + xy.y * dy.x,
                               path.y + xy.x * dx.y + xy.y * dy.y,
                               path.z + xy.x * dx.z + xy.y * dy.z);
        }
    }
}