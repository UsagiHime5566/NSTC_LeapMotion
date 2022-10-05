using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HimeLib.HelpBoxAttribute))]
public class HelpBoxAttributeEditor : PropertyDrawer
{
    HimeLib.HelpBoxAttribute helpAttribute { get { return (HimeLib.HelpBoxAttribute)attribute; } }

    //  Global field to store the original (base) property height.
    float baseHeight = 0;

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
        // We store the original property height for later use...
        baseHeight = base.GetPropertyHeight(prop, label);

        return baseHeight * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        string valueStr;

        switch (prop.propertyType)
        {
            case SerializedPropertyType.Integer:
                valueStr = prop.intValue.ToString();
                break;
            case SerializedPropertyType.Boolean:
                valueStr = prop.boolValue.ToString();
                break;
            case SerializedPropertyType.Float:
                valueStr = prop.floatValue.ToString("0.00000");
                break;
            case SerializedPropertyType.String:
                valueStr = prop.stringValue;
                break;
            default:
                valueStr = "(not supported)";
                break;
        }
        //EditorGUILayout.HelpBox(valueStr, MessageType.Info);     // This is a small box

        EditorGUI.BeginProperty(position, label, prop);
        EditorGUI.HelpBox(position, valueStr, (MessageType) helpAttribute.type);
        EditorGUI.EndProperty();
    }
}