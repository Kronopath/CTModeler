using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CT {
    public class Label : PropertyAttribute {
        public string text;

        public Label(string text) {
            this.text = text;
        }
    }

    [CustomPropertyDrawer(typeof(Label))]
    public class LabelDrawer : DecoratorDrawer {

        string text { get { return ((Label)attribute).text; } }

        GUIStyle richTextStyle = new GUIStyle();

        public LabelDrawer() {
            richTextStyle.richText = true;
        }

        public override float GetHeight() {
            return base.GetHeight() * text.Split('\n').Length;
        }

        public override void OnGUI(Rect position) {
            EditorGUI.LabelField(position, text, richTextStyle);
        }
    }
}