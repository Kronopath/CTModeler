using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CT {
    [CustomPropertyDrawer(typeof(Polyhedron.Triangle))]
    public class PolyhedronTriangleDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            using(var pScope = new EditorGUI.PropertyScope(position, label, property)) {
                
                position = EditorGUI.IndentedRect(position);
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                float width = position.width / 3f;
                float labelWidth = 30f;

                FieldWithLabel(new Rect(position.x, position.y, width, position.height),
                               "V1", labelWidth, property.FindPropertyRelative("v1"));

                FieldWithLabel(new Rect(position.x + width, position.y, width, position.height),
                               "V2", labelWidth, property.FindPropertyRelative("v2"));
                
                FieldWithLabel(new Rect(position.x + width * 2, position.y, width, position.height),
                               "V3", labelWidth, property.FindPropertyRelative("v3"));

                EditorGUI.indentLevel = indentLevel;
            }
        }

        private void FieldWithLabel(Rect position, string label, float labelWidth,
                                    SerializedProperty property)
        {
            EditorGUI.HandlePrefixLabel(
                position,
                new Rect(position.x, position.y, labelWidth, position.height),
                new GUIContent(label));
            
            EditorGUI.PropertyField(
                new Rect(position.x + labelWidth, position.y,
                         position.width - labelWidth - 5, position.height),
                property,
                GUIContent.none);
        }
    }
}