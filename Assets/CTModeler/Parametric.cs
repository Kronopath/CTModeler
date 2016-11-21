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
                vertices[currVertIndex] = f(Mathf.Clamp01(inU), Mathf.Clamp(inV, 0f, 0.999f));

                // Approximate tangents via finite differencing
                Vector3 pu = f(inU + divU / 200, inV);
                Vector3 nu = f(inU - divU / 200, inV);
                Vector3 pv = f(inU, inV + divV / 200);
                Vector3 nv = f(inU, inV - divV / 200);

                Vector3 uTangent = (pu - nu).normalized;
                Vector3 vTangent = (pv - nv).normalized;

                Vector3 normal = Vector3.Cross(vTangent, uTangent).normalized;

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

            Debug.Log(gameObject.name + ": Singularity at (" + inU + ", " + inV + ")");
            Debug.Log(gameObject.name + ": Divisions are (" + divU + ", " + divV + ")");
            Debug.Log(gameObject.name + ": Nearby points are:");
            foreach (Vector2 point in nearbyPoints) {
                Debug.Log(point.ToString("N6"));
            }

            Vector3 averageNormal = Vector3.zero;
            for(int i = 0; i < nearbyPoints.Length; i++) {
                float currU = nearbyPoints[i].x;
                float currV = nearbyPoints[i].y;

                Vector3 pu = f(currU + divU / 200, currV);
                Vector3 nu = f(currU - divU / 200, currV);
                Vector3 pv = f(currU, currV + divV / 200);
                Vector3 nv = f(currU, currV - divV / 200);

                Vector3 uTangent = (pu - nu).normalized;
                Vector3 vTangent = (pv - nv).normalized;

                Vector3 normal = Vector3.Cross(vTangent, uTangent).normalized;
                averageNormal = averageNormal * i / (i + 1) + normal / (i + 1);
            }

            Debug.Log(gameObject.name + ": Avg is " + averageNormal.ToString("N6"));
            return averageNormal.normalized;
        }
    }
}