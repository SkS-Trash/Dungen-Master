using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionaryBase.Storage), true)]
public class SerializableDictionaryStoragePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.Next(true);
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        property.Next(true);
        return EditorGUI.GetPropertyHeight(property);
    }
}