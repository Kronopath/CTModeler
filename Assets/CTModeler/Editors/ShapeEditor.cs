using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CT {
    [CustomEditor(typeof(Shape), true)]
    [CanEditMultipleObjects]
    public class ShapeEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            Shape s = (Shape)target;
            if(GUILayout.Button("Regenerate Mesh")) {
                s.RegenerateMesh();
            }
        }
    }

}