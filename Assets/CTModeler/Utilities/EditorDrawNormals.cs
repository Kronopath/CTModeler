using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// Utility class that draws normals for an object in the editor when selected.
    /// Meant for debugging. Can be pretty slow.
    /// </summary>
    public class EditorDrawNormals : MonoBehaviour {
        private MeshFilter meshFilter;
        public bool shouldDraw = true;

        public void Start() {
            meshFilter = GetComponent<MeshFilter>();
        }

        public void OnDrawGizmosSelected() {
            if(shouldDraw) {
                Gizmos.color = Color.red;
                if(meshFilter != null
                   && meshFilter.mesh != null
                   && meshFilter.mesh.normals != null
                   && meshFilter.mesh.vertices != null
                   && meshFilter.mesh.normals.Length == meshFilter.mesh.vertices.Length)
                {
                    for(int i = 0; i < meshFilter.mesh.vertices.Length; i++) {
                        Vector3 vertexPos = transform.TransformPoint(meshFilter.mesh.vertices[i]);
                        Gizmos.DrawLine(
                            vertexPos,
                            vertexPos + transform.TransformVector(meshFilter.mesh.normals[i]));
                    }
                }
            }
        }
    }
}