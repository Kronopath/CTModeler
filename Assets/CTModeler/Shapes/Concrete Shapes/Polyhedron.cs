using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CT {
    /// <summary>
    /// This class implements a polyhedron shape, defined by a set of manually-specified vertices.
    /// This is somewhat analogous to Unity's Mesh class, except that this does the benefit of
    /// automatically generating some sensible UVs and optionally generating normals for the mesh.
    /// </summary>
    public class Polyhedron : Shape {
        /// <summary>
        /// A helper class that defines a triangle as three vertex indices.
        /// </summary>
        [Serializable]
        public class Triangle {
            public int v1;
            public int v2;
            public int v3;
        }

        /// <summary>
        /// The list of all vertices in the mesh.
        /// </summary>
        public Vector3[] vertices;
        /// <summary>
        /// All triangles in the mesh, specified as the indices of all the vertices.
        /// </summary>
        public Triangle[] triangles;

        [Label("<b>Note:</b> The Normals array is optional.\n"+
               "If no normals are supplied, they will be auto-generated.")]
        /// <summary>
        /// An array containing the normals of all the vertices. Note that these correspond to the
        /// order they're in on the triangles array, not the vertices array, so that you can, for 
        /// example, only enter 8 vertices for a cube but still have each face of the cube have distinct
        /// normals. Thus, the size of this array should be three times the size of the triangles array.
        /// If this array is empty this class will attempt to generate normals for the mesh.
        /// </summary>
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