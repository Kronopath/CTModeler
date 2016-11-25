using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// Class for parametric objects defined by extruding a shape along a path.
    /// </summary>
    public abstract class Extruded : Parametric {

        /// <summary>
        /// This function should be overriden to define the 2D profile of the shape being extruded.
        /// 
        /// For example, a simple circle would just be returning (cos(2pi*u), sin(2pi*u)).
        /// </summary>
        /// <returns>A position on the XY plane for the given u and v parameters.</returns>
        /// <param name="u">Goes from 0 to 1 perpendicular to the path.</param>
        /// <param name="v">Goes from 0 at the beginning of the path to 1 at the end of the path.
        /// </param>
        protected abstract Vector2 ExtrusionProfileFunction(float u, float v);

        /// <summary>
        /// This function should be overriden to define the path the shape is being extruded along.
        /// </summary>
        /// <returns>The point in 3D space where the path would be at a given v.</returns>
        /// <param name="v">Goes from 0 at the beginning of the path to 1 at the end of it.</param>
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