using UnityEngine;
using System;
using System.Collections;

namespace CT {
    /// <summary>
    /// Base class for a shape defined by a parametric function.
    /// </summary>
    public abstract class Parametric : Shape {
        /// <summary>
        /// The number of triangles to have along the direction of the U parameter.
        /// </summary>
        public int numU = 30;
        /// <summary>
        /// The number of triangles to have along the direction of the V parameter.
        /// </summary>
        public int numV = 30;

        /// <summary>
        /// Override this function to implement the parametric function that defines this mesh.
        /// </summary>
        /// <returns>The surface of the mesh at the given U and V parametric coordinates.</returns>
        /// <param name="u">The first parametric coordinate, varies between 0 and 1.</param>
        /// <param name="v">The second parametric coordinate, varies between 0 and 1.</param>
        protected abstract Vector3 ParametricFunction(float u, float v);

        protected sealed override Mesh CreateMesh() {
            float divU = 1.0f / numU;
            float divV = 1.0f / numV;

            int numVertices = (numU + 1) * 2 * numV + numV;
            Vector3[] vertices = new Vector3[numVertices];
            Vector4[] tangents = new Vector4[numVertices];
            Vector3[] normals = new Vector3[numVertices];
            Vector2[] uvs = new Vector2[numVertices];
            int[] triangles = GenerateTriangleStripIndices(numVertices - 2);

            int currVertIndex = 0;

            Action<float, float> addVertex = (float inU, float inV) => {
                // Compute parametric vertex
                vertices[currVertIndex] = ParametricFunction(Mathf.Clamp01(inU), Mathf.Clamp01(inV));

                Vector3 uTangent = Vector3.zero;
                Vector3 vTangent = Vector3.zero;
                Vector3 normal = Vector3.zero;
                GetNormalAndTangents(inU, inV, divU, divV, out uTangent, out vTangent, out normal);

                if(normal == Vector3.zero) {
                    // We're likely at a singularity or pole in the model. 
                    normal = GetNormalAtSingularity(inU, inV, divU, divV);
                }

                tangents[currVertIndex] = uTangent;
                normals[currVertIndex] = normal;
                uvs[currVertIndex] = new Vector2(inU, inV);

                currVertIndex++;
            };

            float u = 0;
            float d = divU;
            // Zigzag across rows to form a triangle strip.
            for(int j = 0; j < numV; j++) {
                float v = j * divV;
                for(int i = 0; i <= numU; i++) {
                    addVertex(u, v);
                    addVertex(u, v + divV);
                    if(i < numU) {
                        u += d;
                    }
                    else {
                        addVertex(u, v);
                    }
                }
                d = -d;
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.tangents = tangents;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            return mesh;
        }

        /// <summary>
        /// If you're at a pole in the model, like the ends of a sphere, you can't take the 
        /// cross product of the two tangents to get the normal, because varying one of the
        /// parametric directions leaves you at the same location in 3D space, leading one of
        /// the tangents to be zero when attempting to calculate them by finite differencing.
        /// To remedy this, we take the normals at 4 points near the pole and average them, to try
        /// and get an approximation of the normal.
        /// </summary>
        /// <returns>An approximation of the normal at the singularity.</returns>
        /// <param name="inU">U parametric coordinate.</param>
        /// <param name="inV">V parametric coordinate.</param>
        /// <param name="divU">The spacing between vertices along the U axis.</param>
        /// <param name="divV">The spacing between vertices along the V axis.</param>
        private Vector3 GetNormalAtSingularity(float inU, float inV, float divU, float divV)
        {
            // Take the normal at four different points nearby this vertex, then average them.
            Vector2[] nearbyPoints = {
                new Vector2(Mathf.Clamp01(inU + divU / 100), Mathf.Clamp01(inV + divV / 100)),
                new Vector2(Mathf.Clamp01(inU + divU / 100), Mathf.Clamp01(inV - divV / 100)),
                new Vector2(Mathf.Clamp01(inU - divU / 100), Mathf.Clamp01(inV - divV / 100)),
                new Vector2(Mathf.Clamp01(inU - divU / 100), Mathf.Clamp01(inV + divV / 100))
            };

            Vector3 averageNormal = Vector3.zero;
            for(int i = 0; i < nearbyPoints.Length; i++) {
                float currU = nearbyPoints[i].x;
                float currV = nearbyPoints[i].y;

                Vector3 uTangent = Vector3.zero;
                Vector3 vTangent = Vector3.zero;
                Vector3 normal = Vector3.zero;
                GetNormalAndTangents(currU, currV, divU, divV, out uTangent, out vTangent, out normal);

                averageNormal = averageNormal * i / (i + 1) + normal / (i + 1);
            }

            return averageNormal.normalized;
        }

        /// <summary>
        /// Gets the normal and tangents of the surface at the given parametric coordinates.
        /// </summary>
        /// <param name="inU">U parametric coordinate.</param>
        /// <param name="inV">V parametric coordinate.</param>
        /// <param name="divU">The spacing between vertices along the U axis.</param>
        /// <param name="divV">The spacing between vertices along the V axis.</param>
        /// <param name="uTangent">Gets set to the tangent in the u direction.</param>
        /// <param name="vTangent">Gets set to the tangent in the v direction.</param>
        /// <param name="normal">Gets set to the normal vector.</param>
        private void GetNormalAndTangents(float inU, float inV, float divU, float divV,
                                          out Vector3 uTangent, out Vector3 vTangent,
                                          out Vector3 normal)
        {
            // Approximate tangents via finite differencing
            Vector3 pu = ParametricFunction(inU + divU / 200, inV);
            Vector3 nu = ParametricFunction(inU - divU / 200, inV);
            Vector3 pv = ParametricFunction(inU, inV + divV / 200);
            Vector3 nv = ParametricFunction(inU, inV - divV / 200);

            uTangent = (pu - nu).normalized;
            vTangent = (pv - nv).normalized;
            normal = Vector3.Cross(vTangent, uTangent).normalized;
        }
    }
}