using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CT {
    /// <summary>
    /// Custom inspector interface for all CT Shapes. Keeps most everything the same, but adds
    /// a "Regenerate Mesh" button to the bottom of the inspector to allow you to manually regenerate
    /// the mesh for any shape when needed.
    /// </summary>
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