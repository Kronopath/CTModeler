using UnityEngine;
using System;
using System.Collections;

namespace CT {
    public abstract class Parametric : Shape {
        public int numU = 30;
        public int numV = 30;

        protected abstract Vector3 ParametricFunction(float u, float v);

        protected override Mesh CreateMesh() {
            return CreateParametricSurface(numU, numV, ParametricFunction);
        }

        protected Mesh CreateParametricSurface(int numU, int numV, Func<float, float, Vector3> f) {
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
                vertices[currVertIndex] = f(Mathf.Clamp01(inU), Mathf.Clamp01(inV));

                Vector3 uTangent = Vector3.zero;
                Vector3 vTangent = Vector3.zero;
                Vector3 normal = Vector3.zero;
                GetNormalAndTangents(f, inU, inV, divU, divV, out uTangent, out vTangent, out normal);

                if(normal == Vector3.zero) {
                    // We're likely at a singularity or pole in the model. 
                    normal = GetNormalAtSingularity(f, inU, inV, divU, divV);
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

        private Vector3 GetNormalAtSingularity(Func<float, float, Vector3> f,
                                               float inU, float inV, float divU, float divV)
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
                GetNormalAndTangents(f, currU, currV, divU, divV, out uTangent, out vTangent,
                                     out normal);

                averageNormal = averageNormal * i / (i + 1) + normal / (i + 1);
            }

            return averageNormal.normalized;
        }

        private void GetNormalAndTangents(Func<float, float, Vector3> f,
                                          float inU, float inV, float divU, float divV,
                                          out Vector3 uTangent, out Vector3 vTangent,
                                          out Vector3 normal)
        {
            // Approximate tangents via finite differencing
            Vector3 pu = f(inU + divU / 200, inV);
            Vector3 nu = f(inU - divU / 200, inV);
            Vector3 pv = f(inU, inV + divV / 200);
            Vector3 nv = f(inU, inV - divV / 200);

            uTangent = (pu - nu).normalized;
            vTangent = (pv - nv).normalized;
            normal = Vector3.Cross(vTangent, uTangent).normalized;
        }
    }
}