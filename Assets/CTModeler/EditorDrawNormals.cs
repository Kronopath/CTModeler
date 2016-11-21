using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// Utility class that draws normals for an object in the editor when selected.
    /// Meant for debugging.
    /// </summary>
    public class EditorDrawNormals : MonoBehaviour {
        private MeshFilter meshFilter;

        public void Start() {
            meshFilter = GetComponent<MeshFilter>();
    }

        public void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            if(meshFilter && meshFilter.mesh) {
                for(int i = 0; i < meshFilter.mesh.vertices.Length; i++) {
                    Vector3 vertexPos = transform.TransformPoint(meshFilter.mesh.vertices[i]);
                    Gizmos.DrawLine(vertexPos,
                                    vertexPos + transform.TransformVector(meshFilter.mesh.normals[i]));
                }
            }
    }
    }
}