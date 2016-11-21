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

                // If either of the tangents are zero, that means we're at a singularity in the mesh.
                // This would lead to a zero vector as our normal if left as-is.
                // So, instead, take the tangent that's zero and replace it by another tangent along
                // the other parametric axis that's a quarter of the way around the singularity.
                if(pu == nu) {
                    pu = f((inU + 0.25f) % 1.0f, inV + divV / 200);
                    nu = f((inU + 0.25f) % 1.0f, inV - divV / 200);
                }
                if(pv == nv) {
                    pv = f(inU + divU / 200, (inV + 0.25f) % 1.0f);
                    nv = f(inU - divU / 200, (inV + 0.25f) % 1.0f);
                }
                // If they're both zero, then god help you, you're trying to model a black hole.
                // Good luck with that.

                Vector3 uTangent = (pu - nu).normalized;
                Vector3 vTangent = (pv - nv).normalized;

                Vector3 normal = Vector3.Cross(vTangent, uTangent).normalized;
                if(normal == Vector3.zero) {
                    Debug.LogError("Zero normal for " + this.gameObject.name + " at (" + inU + ", " + inV + "), tangents " + uTangent + ", " + vTangent);
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
    }
}