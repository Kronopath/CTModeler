using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CT {
    [CustomEditor(typeof(Shape), true)]
    [CanEditMultipleObjects]
    public class ShapeEditor : Editor {
        public override void OnInspectorGUI() {
            serializedObject.Update();
            DrawDefaultInspector();

            Shape s = (Shape)target;
            if(GUILayout.Button("Regenerate Mesh")) {
                s.RegenerateMesh();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

}