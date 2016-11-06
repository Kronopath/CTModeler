using UnityEngine;
using System.Collections;

namespace CT {
    
    public class Cube : Shape {
        protected override Mesh CreateMesh() {
            Vector3[] vertices = new Vector3[6 * 4];
            Vector4[] tangents = new Vector4[6 * 4];
            Vector3[] normals = new Vector3[6 * 4];
            Vector2[] uvs = new Vector2[6 * 4];
            int[] triangles = new int[6 * 2 * 3];

            Vector3[,] faces = new Vector3[6, 3] {
                { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) },
                { new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0) },
                { new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 1, 0) },
                { new Vector3(0, 0, -1), new Vector3(-1, 0, 0), new Vector3(0, 1, 0) },
                { new Vector3(0, -1, 0), new Vector3(0, 0, -1), new Vector3(1, 0, 0) },
                { new Vector3(-1, 0, 0), new Vector3(0, -1, 0), new Vector3(0, 0, 1) },
            };

            Mesh mesh = new Mesh();

            for(int i = 0; i < 6; i++) {
                vertices[i * 4 + 0] = faces[i, 0] - faces[i, 1] - faces[i, 2];
                vertices[i * 4 + 1] = faces[i, 0] + faces[i, 1] - faces[i, 2];
                vertices[i * 4 + 2] = faces[i, 0] + faces[i, 1] + faces[i, 2];
                vertices[i * 4 + 3] = faces[i, 0] - faces[i, 1] + faces[i, 2];

                triangles[i * 6 + 0] = i * 4 + 0;
                triangles[i * 6 + 1] = i * 4 + 1;
                triangles[i * 6 + 2] = i * 4 + 2;

                triangles[i * 6 + 3] = i * 4 + 3;
                triangles[i * 6 + 4] = i * 4 + 0;
                triangles[i * 6 + 5] = i * 4 + 2;

                tangents[i * 4 + 0] = faces[i, 1];
                tangents[i * 4 + 1] = faces[i, 1];
                tangents[i * 4 + 2] = faces[i, 1];
                tangents[i * 4 + 3] = faces[i, 1];

                normals[i * 4 + 0] = faces[i, 0];
                normals[i * 4 + 1] = faces[i, 0];
                normals[i * 4 + 2] = faces[i, 0];
                normals[i * 4 + 3] = faces[i, 0];

                uvs[i * 4 + 0] = new Vector2(0, 0);
                uvs[i * 4 + 1] = new Vector2(1, 0);
                uvs[i * 4 + 2] = new Vector2(1, 1);
                uvs[i * 4 + 3] = new Vector2(0, 1);
            }

            mesh.vertices = vertices;
            mesh.tangents = tangents;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            return mesh;
        }
    }
}