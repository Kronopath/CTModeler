using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CT {
    public class Polyhedron : Shape {
        [Serializable]
        public class Triangle {
            public int v1;
            public int v2;
            public int v3;
        }

        public Vector3[] vertices;
        public Triangle[] triangles;

        [Label("<b>Note:</b> The Normals array is optional.\n"+
               "If no normals are supplied, they will be auto-generated.")]
        public Vector3[] normals;

        protected override Mesh CreateMesh() {
            List<Vector3> finalVertexList = new List<Vector3>();
            List<Vector3> finalNormalList = new List<Vector3>();
            List<Vector4> finalTangentList = new List<Vector4>();
            List<Vector2> finalUVList = new List<Vector2>();
            int parity = 1;

            foreach(Triangle tri in triangles) {
                AddTriangle(tri, parity, finalVertexList, finalNormalList,
                            finalTangentList, finalUVList);
                parity = 1 - parity;
            }

            Mesh mesh = new Mesh();

            mesh.vertices = finalVertexList.ToArray();
            mesh.normals = finalNormalList.ToArray();
            mesh.tangents = finalTangentList.ToArray();
            mesh.uv = finalUVList.ToArray();

            int[] indexList = new int[triangles.Length*3];
            for (int i = 0; i < indexList.Length; i++) {
                indexList[i] = i;
            }
            mesh.triangles = indexList;

            return mesh;
        }

        private void AddTriangle(Triangle t, int parity,
                                 List<Vector3> vertexListToAddTo, List<Vector3> normalListToAddTo,
                                 List<Vector4> tangentListToAddTo, List<Vector2> uvListToAddTo)
        {
            Vector3 a = vertices[t.v1];
            Vector3 b = vertices[t.v2];
            Vector3 c = vertices[t.v3];
            Vector4 tangent = (b - a).normalized;
            Vector3 genNormal = Vector3.Cross(tangent, c - b).normalized;

            vertexListToAddTo.Add(a);
            vertexListToAddTo.Add(b);
            vertexListToAddTo.Add(c);

            int n = normalListToAddTo.Count;
            normalListToAddTo.Add(normals.Length > n ? normals[n].normalized : genNormal);
            normalListToAddTo.Add(normals.Length > n+1 ? normals[n+1].normalized : genNormal);
            normalListToAddTo.Add(normals.Length > n+2 ? normals[n+2].normalized : genNormal);

            tangentListToAddTo.Add(tangent);
            tangentListToAddTo.Add(tangent);
            tangentListToAddTo.Add(tangent);

            uvListToAddTo.Add(new Vector2(parity, parity));
            uvListToAddTo.Add(new Vector2(parity, 1 - parity));
            uvListToAddTo.Add(new Vector2(1 - parity, 1 - parity));
        }
    }
}