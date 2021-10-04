using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HideIfAttribute))]
public class HideIfPropertyDrawer : UnityEditor.PropertyDrawer
{
    bool GetConditionalValue(HideIfAttribute attr, SerializedProperty property)
    {
        var fieldProperty = property.serializedObject.FindProperty(attr.conditionalFieldName);

        if (fieldProperty != null)
        {
            return attr.reverseConditional ? !fieldProperty.boolValue : fieldProperty.boolValue;
        }
        else
        {
            return true;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var hideIfAttr = attribute as HideIfAttribute;
        bool enabled = GetConditionalValue(hideIfAttr, property);

        if (enabled)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var hideIfAttr = attribute as HideIfAttribute;
        bool enabled = GetConditionalValue(hideIfAttr, property);

        if (enabled)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        else
        {
            return EditorGUIUtility.standardVerticalSpacing;
        }
    }
}