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
                Vector3 normal = Vector3.Cross(vTangent, uTangent);

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
    }
}