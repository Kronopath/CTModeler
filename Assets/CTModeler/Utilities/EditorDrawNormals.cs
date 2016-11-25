using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// Utility class that draws normals for an object in the editor when selected.
    /// Meant for debugging. Can be pretty slow, so be careful.
    /// </summary>
    public class EditorDrawNormals : MonoBehaviour {
        /// <summary>
        /// Turns normal drawing on or off.
        /// </summary>
        public bool shouldDraw = true;

        private MeshFilter meshFilter;

        public void OnDrawGizmosSelected() {
            if(shouldDraw) {
                Gizmos.color = Color.red;
                if (meshFilter == null) {
                    meshFilter = GetComponent<MeshFilter>();
                }
                if(meshFilter != null
                   && meshFilter.sharedMesh != null
                   && meshFilter.sharedMesh.normals != null
                   && meshFilter.sharedMesh.vertices != null
                   && meshFilter.sharedMesh.normals.Length == meshFilter.sharedMesh.vertices.Length)
                {
                    for(int i = 0; i < meshFilter.sharedMesh.vertices.Length; i++) {
                        Vector3 vertexPos = transform.TransformPoint(meshFilter.sharedMesh.vertices[i]);
                        Gizmos.DrawLine(
                            vertexPos,
                            vertexPos + transform.TransformVector(meshFilter.sharedMesh.normals[i]));
                    }
                }
            }
        }

        public void OnValidate() {
            meshFilter = GetComponent<MeshFilter>();
        }
    }
}