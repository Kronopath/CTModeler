using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CT {
    [CustomPropertyDrawer(typeof(Polyhedron.Triangle))]
    public class PolyhedronTriangleDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            using (var pScope = new EditorGUI.PropertyScope(position, label, property)) {
                float width = position.width/3f;

                EditorGUI.PropertyField(new Rect(position.x, position.y, width, position.height),
                                        property.FindPropertyRelative("v1"),
                                        GUIContent.none);
                
                EditorGUI.PropertyField(new Rect(position.x+width, position.y,
                                                 width, position.height),
                                        property.FindPropertyRelative("v2"),
                                        GUIContent.none);

                EditorGUI.PropertyField(new Rect(position.x+width*2, position.y,
                                                 width, position.height),
                                        property.FindPropertyRelative("v3"),
                                        GUIContent.none);
            }
        }
    }
}