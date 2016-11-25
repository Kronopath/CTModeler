using UnityEngine;
using UnityEditor;
using System.Collections;

namespace CT {
    /// <summary>
    /// Add this attribute to a public variable on a MonoBehaviour to allow for text to be
    /// printed above the variable in the Unity inspector. Supports Unity's limited rich text
    /// subset of HTML.
    /// 
    /// Example usage:
    /// 
    /// [Label("This text will be shown above Num Things in the inspector")]
    /// public int numThings;
    /// </summary>
    public class Label : PropertyAttribute {
        public string text;

        public Label(string text) {
            this.text = text;
        }
    }

    /// <summary>
    /// Decorator drawer that takes care of actually drawing the text from the above Label attribute.
    /// </summary>
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