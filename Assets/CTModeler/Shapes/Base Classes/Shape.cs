using UnityEngine;
using System.Collections;

namespace CT {
    /// <summary>
    /// This is the base class for all shapes in the CT modeler.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class Shape : MonoBehaviour {

        /// <summary>
        /// Turn this on in the inspector to allow the mesh to be regenerated every time a variable
        /// is changed in the inspector. If turned off, the shape waits for you to click "Regenerate
        /// mesh" first.
        /// </summary>
        public bool liveUpdatingInEditor = true;

        /// <summary>
        /// This function is called whenever this object needs to regenerate its mesh.
        /// You should override this in your derived classes and return a mesh with the correct
        /// vertices, normals, UVs, and so on.
        /// </summary>
        /// <returns>The mesh you want this object to have.</returns>
        protected abstract Mesh CreateMesh();

        /// <summary>
        /// Standard initialization function.
        /// </summary>
        public virtual void Start() {
            RegenerateMesh();
        }
	
        /// <summary>
        /// Standard update function.
        /// </summary>
        public virtual void Update() {
        }

        /// <summary>
        /// Generates a set of indices that are suitable for assigning to the mesh.triangles array
        /// for any object that has its vertices in a triangle strip.
        /// </summary>
        /// <returns>The full list of indices for all triangles.</returns>
        /// <param name="numTris">The number of triangles you have.</param>
        protected int[] GenerateTriangleStripIndices(int numTris) {
            int[] tris = new int[numTris * 3];
            for(int i = 0; i < numTris; i++) {
                if(i % 2 == 0) {
                    tris[i * 3] = i;
                    tris[i * 3 + 1] = i + 1;
                    tris[i * 3 + 2] = i + 2;
                }
                else {
                    tris[i * 3] = i + 1;
                    tris[i * 3 + 1] = i;
                    tris[i * 3 + 2] = i + 2;
                }
            }
            return tris;
        }

        /// <summary>
        /// This is called every time a value is changed in the inspector.
        /// </summary>
        public virtual void OnValidate() {
            if(liveUpdatingInEditor) {
                RegenerateMesh();
            }
        }

        /// <summary>
        /// Recalculates and regenerates the mesh for this shape.
        /// </summary>
        public void RegenerateMesh() {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = CreateMesh();
        }
    }
}