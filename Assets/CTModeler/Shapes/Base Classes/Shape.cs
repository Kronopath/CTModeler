using UnityEngine;
using System.Collections;

namespace CT {
    
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public abstract class Shape : MonoBehaviour {

        public bool liveUpdatingInEditor = true;

        // Use this for initialization
        public virtual void Start() {
            RegenerateMesh();
        }
	
        // Update is called once per frame
        public virtual void Update() {
        }

        protected abstract Mesh CreateMesh();

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

        public void OnValidate() {
            if(liveUpdatingInEditor) {
                RegenerateMesh();
            }
        }

        public void RegenerateMesh() {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = CreateMesh();
        }
    }
}