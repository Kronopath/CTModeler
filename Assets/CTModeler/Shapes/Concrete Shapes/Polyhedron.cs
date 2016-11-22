using UnityEngine;
using System;
using System.Collections;

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
        // TODO: add to name in inspector, call it "optional"
        public Vector3[] normals;

        protected override Mesh CreateMesh() {
            Mesh mesh = new Mesh();

            mesh.vertices = vertices;

            int[] flatTriangles = new int[triangles.Length * 3];
            for(int i = 0; i < triangles.Length; i++) {
                flatTriangles[i * 3] = triangles[i].v1;
                flatTriangles[i * 3 + 1] = triangles[i].v2;
                flatTriangles[i * 3 + 2] = triangles[i].v3;
            }
            mesh.triangles = flatTriangles;

            if (normals.Length == vertices.Length) {
                mesh.normals = normals;
                for (int i = 0; i < mesh.normals.Length; i++) {
                    mesh.normals[i] = mesh.normals[i].normalized;
                }
            }
            else {
                if (normals.Length > 0) {
                    Debug.LogError(gameObject.name + ".Polyhedron: Number of normals ("
                                   + normals.Length + ") does not match number of vertices ("
                                   + vertices.Length + ").");
                }
            }

            // TODO: normals and UVs
            return mesh;
        }
    }
}