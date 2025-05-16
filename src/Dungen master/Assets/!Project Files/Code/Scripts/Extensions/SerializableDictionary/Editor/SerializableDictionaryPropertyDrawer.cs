using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionaryBase), true)]
#if NET_4_6 || NET_STANDARD_2_0
[CustomPropertyDrawer(typeof(SerializableHashSetBase), true)]
#endif
public class SerializableDictionaryPropertyDrawer : PropertyDrawer
{
    private const string KeysFieldName = "m_keys";
    private const string ValuesFieldName = "m_values";
    protected const float IndentWidth = 15f;

    private static GUIContent s_iconPlus = IconContent("Toolbar Plus", "Add entry");
    private static GUIContent s_iconMinus = IconContent("Toolbar Minus", "Remove entry");

    private static GUIContent s_warningIconConflict =
        IconContent("console.warnicon.sml", "Conflicting key, this entry will be lost");

    private static GUIContent s_warningIconOther = IconContent("console.infoicon.sml", "Conflicting key");

    private static GUIContent s_warningIconNull =
        IconContent("console.warnicon.sml", "Null key, this entry will be lost");

    private static GUIStyle s_buttonStyle = GUIStyle.none;
    private static GUIContent s_tempContent = new();


    private class ConflictState
    {
        public object conflictKey = null;
        public object conflictValue = null;
        public int conflictIndex = -1;
        public int conflictOtherIndex = -1;
        public bool conflictKeyPropertyExpanded = false;
        public bool conflictValuePropertyExpanded = false;
        public float conflictLineHeight = 0f;
    }

    private struct PropertyIdentity
    {
        public PropertyIdentity(SerializedProperty property)
        {
            instance = property.serializedObject.targetObject;
            propertyPath = property.propertyPath;
        }

        public UnityEngine.Object instance;
        public string propertyPath;
    }

    private static Dictionary<PropertyIdentity, ConflictState> s_conflictStateDict = new();

    private enum Action
    {
        None,
        Add,
        Remove
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);

        var buttonAction = Action.None;
        var buttonActionIndex = 0;

        var keyArrayProperty = property.FindPropertyRelative(KeysFieldName);
        var valueArrayProperty = property.FindPropertyRelative(ValuesFieldName);

        var conflictState = GetConflictState(property);

        if (conflictState.conflictIndex != -1)
        {
            keyArrayProperty.InsertArrayElementAtIndex(conflictState.conflictIndex);
            var keyProperty = keyArrayProperty.GetArrayElementAtIndex(conflictState.conflictIndex);
            SetPropertyValue(keyProperty, conflictState.conflictKey);
            keyProperty.isExpanded = conflictState.conflictKeyPropertyExpanded;

            if (valueArrayProperty != null)
            {
                valueArrayProperty.InsertArrayElementAtIndex(conflictState.conflictIndex);
                var valueProperty = valueArrayProperty.GetArrayElementAtIndex(conflictState.conflictIndex);
                SetPropertyValue(valueProperty, conflictState.conflictValue);
                valueProperty.isExpanded = conflictState.conflictValuePropertyExpanded;
            }
        }

        var buttonWidth = s_buttonStyle.CalcSize(s_iconPlus).x;

        var labelPosition = position;
        labelPosition.height = EditorGUIUtility.singleLineHeight;
        if (property.isExpanded)
            labelPosition.xMax -= s_buttonStyle.CalcSize(s_iconPlus).x;

        EditorGUI.PropertyField(labelPosition, property, label, false);

        if (property.isExpanded)
        {
            var buttonPosition = position;
            buttonPosition.xMin = buttonPosition.xMax - buttonWidth;
            buttonPosition.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.BeginDisabledGroup(conflictState.conflictIndex != -1);
            if (GUI.Button(buttonPosition, s_iconPlus, s_buttonStyle))
            {
                buttonAction = Action.Add;
                buttonActionIndex = keyArrayProperty.arraySize;
            }

            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel++;
            var linePosition = position;
            linePosition.y += EditorGUIUtility.singleLineHeight;
            linePosition.xMax -= buttonWidth;

            foreach (var entry in EnumerateEntries(keyArrayProperty, valueArrayProperty))
            {
                var keyProperty = entry.keyProperty;
                var valueProperty = entry.valueProperty;
                var i = entry.index;

                var lineHeight = DrawKeyValueLine(keyProperty, valueProperty, linePosition, i);

                buttonPosition = linePosition;
                buttonPosition.x = linePosition.xMax;
                buttonPosition.height = EditorGUIUtility.singleLineHeight;
                if (GUI.Button(buttonPosition, s_iconMinus, s_buttonStyle))
                {
                    buttonAction = Action.Remove;
                    buttonActionIndex = i;
                }

                if (i == conflictState.conflictIndex && conflictState.conflictOtherIndex == -1)
                {
                    var iconPosition = linePosition;
                    iconPosition.size = s_buttonStyle.CalcSize(s_warningIconNull);
                    GUI.Label(iconPosition, s_warningIconNull);
                }
                else if (i == conflictState.conflictIndex)
                {
                    var iconPosition = linePosition;
                    iconPosition.size = s_buttonStyle.CalcSize(s_warningIconConflict);
                    GUI.Label(iconPosition, s_warningIconConflict);
                }
                else if (i == conflictState.conflictOtherIndex)
                {
                    var iconPosition = linePosition;
                    iconPosition.size = s_buttonStyle.CalcSize(s_warningIconOther);
                    GUI.Label(iconPosition, s_warningIconOther);
                }


                linePosition.y += lineHeight;
            }

            EditorGUI.indentLevel--;
        }

        switch (buttonAction)
        {
            case Action.Add:
            {
                keyArrayProperty.InsertArrayElementAtIndex(buttonActionIndex);
                if (valueArrayProperty != null)
                    valueArrayProperty.InsertArrayElementAtIndex(buttonActionIndex);
                break;
            }
            case Action.Remove:
            {
                DeleteArrayElementAtIndex(keyArrayProperty, buttonActionIndex);
                if (valueArrayProperty != null)
                    DeleteArrayElementAtIndex(valueArrayProperty, buttonActionIndex);
                break;
            }
        }

        conflictState.conflictKey = null;
        conflictState.conflictValue = null;
        conflictState.conflictIndex = -1;
        conflictState.conflictOtherIndex = -1;
        conflictState.conflictLineHeight = 0f;
        conflictState.conflictKeyPropertyExpanded = false;
        conflictState.conflictValuePropertyExpanded = false;

        foreach (var entry1 in EnumerateEntries(keyArrayProperty, valueArrayProperty))
        {
            var keyProperty1 = entry1.keyProperty;
            var i = entry1.index;
            var keyProperty1Value = GetPropertyValue(keyProperty1);

            if (keyProperty1Value == null)
            {
                var valueProperty1 = entry1.valueProperty;
                SaveProperty(keyProperty1, valueProperty1, i, -1, conflictState);
                DeleteArrayElementAtIndex(keyArrayProperty, i);
                if (valueArrayProperty != null)
                    DeleteArrayElementAtIndex(valueArrayProperty, i);

                break;
            }


            foreach (var entry2 in EnumerateEntries(keyArrayProperty, valueArrayProperty, i + 1))
            {
                var keyProperty2 = entry2.keyProperty;
                var j = entry2.index;
                var keyProperty2Value = GetPropertyValue(keyProperty2);

                if (!ComparePropertyValues(keyProperty1Value, keyProperty2Value)) continue;
                var valueProperty2 = entry2.valueProperty;
                SaveProperty(keyProperty2, valueProperty2, j, i, conflictState);
                DeleteArrayElementAtIndex(keyArrayProperty, j);
                if (valueArrayProperty != null)
                    DeleteArrayElementAtIndex(valueArrayProperty, j);

                goto breakLoops;
            }
        }

        breakLoops:

        EditorGUI.EndProperty();
    }

    private static float DrawKeyValueLine(SerializedProperty keyProperty, SerializedProperty valueProperty,
        Rect linePosition, int index)
    {
        var keyCanBeExpanded = CanPropertyBeExpanded(keyProperty);

        string keyLabel;
        if (valueProperty != null)
        {
            var valueCanBeExpanded = CanPropertyBeExpanded(valueProperty);

            if (!keyCanBeExpanded && valueCanBeExpanded)
            {
                return DrawKeyValueLineExpand(keyProperty, valueProperty, linePosition);
            }

            keyLabel = keyCanBeExpanded ? "Key " + index : "";
            var valueLabel = valueCanBeExpanded ? ("Value " + index) : "";
            return DrawKeyValueLineSimple(keyProperty, valueProperty, keyLabel, valueLabel, linePosition);
        }

        if (!keyCanBeExpanded)
        {
            return DrawKeyLine(keyProperty, linePosition, null);
        }

        keyLabel = $"{ObjectNames.NicifyVariableName(keyProperty.type)} {index}";
        return DrawKeyLine(keyProperty, linePosition, keyLabel);
    }

    private static float DrawKeyValueLineSimple(SerializedProperty keyProperty, SerializedProperty valueProperty,
        string keyLabel, string valueLabel, Rect linePosition)
    {
        var labelWidth = EditorGUIUtility.labelWidth;
        var labelWidthRelative = labelWidth / linePosition.width;

        var keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
        var keyPosition = linePosition;
        keyPosition.height = keyPropertyHeight;
        keyPosition.width = labelWidth - IndentWidth;
        EditorGUIUtility.labelWidth = keyPosition.width * labelWidthRelative;
        EditorGUI.PropertyField(keyPosition, keyProperty, TempContent(keyLabel), true);

        var valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
        var valuePosition = linePosition;
        valuePosition.height = valuePropertyHeight;
        valuePosition.xMin += labelWidth;
        EditorGUIUtility.labelWidth = valuePosition.width * labelWidthRelative;
        EditorGUI.indentLevel--;
        EditorGUI.PropertyField(valuePosition, valueProperty, TempContent(valueLabel), true);
        EditorGUI.indentLevel++;

        EditorGUIUtility.labelWidth = labelWidth;

        return Mathf.Max(keyPropertyHeight, valuePropertyHeight);
    }

    private static float DrawKeyValueLineExpand(SerializedProperty keyProperty, SerializedProperty valueProperty,
        Rect linePosition)
    {
        var labelWidth = EditorGUIUtility.labelWidth;

        var keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
        var keyPosition = linePosition;
        keyPosition.height = keyPropertyHeight;
        keyPosition.width = labelWidth - IndentWidth;
        EditorGUI.PropertyField(keyPosition, keyProperty, GUIContent.none, true);

        var valuePropertyHeight = EditorGUI.GetPropertyHeight(valueProperty);
        var valuePosition = linePosition;
        valuePosition.height = valuePropertyHeight;
        EditorGUI.PropertyField(valuePosition, valueProperty, GUIContent.none, true);

        EditorGUIUtility.labelWidth = labelWidth;

        return Mathf.Max(keyPropertyHeight, valuePropertyHeight);
    }

    private static float DrawKeyLine(SerializedProperty keyProperty, Rect linePosition, string keyLabel)
    {
        var keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
        var keyPosition = linePosition;
        keyPosition.height = keyPropertyHeight;
        keyPosition.width = linePosition.width;

        var keyLabelContent = keyLabel != null ? TempContent(keyLabel) : GUIContent.none;
        EditorGUI.PropertyField(keyPosition, keyProperty, keyLabelContent, true);

        return keyPropertyHeight;
    }

    private static bool CanPropertyBeExpanded(SerializedProperty property) => property.propertyType switch
    {
        SerializedPropertyType.Generic or
            SerializedPropertyType.Vector4 or
            SerializedPropertyType.Quaternion => true,
        _ => false
    };

    private static void SaveProperty(SerializedProperty keyProperty, SerializedProperty valueProperty, int index,
        int otherIndex, ConflictState conflictState)
    {
        conflictState.conflictKey = GetPropertyValue(keyProperty);
        conflictState.conflictValue = valueProperty != null ? GetPropertyValue(valueProperty) : null;
        var keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty);
        var valuePropertyHeight = valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty) : 0f;
        var lineHeight = Mathf.Max(keyPropertyHeight, valuePropertyHeight);
        conflictState.conflictLineHeight = lineHeight;
        conflictState.conflictIndex = index;
        conflictState.conflictOtherIndex = otherIndex;
        conflictState.conflictKeyPropertyExpanded = keyProperty.isExpanded;
        conflictState.conflictValuePropertyExpanded = valueProperty?.isExpanded ?? false;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var propertyHeight = EditorGUIUtility.singleLineHeight;

        if (!property.isExpanded) return propertyHeight;
        var keysProperty = property.FindPropertyRelative(KeysFieldName);
        var valuesProperty = property.FindPropertyRelative(ValuesFieldName);

        propertyHeight += (from entry in EnumerateEntries(keysProperty, valuesProperty)
            let keyProperty = entry.keyProperty
            let valueProperty = entry.valueProperty
            let keyPropertyHeight = EditorGUI.GetPropertyHeight(keyProperty)
            let valuePropertyHeight = valueProperty != null ? EditorGUI.GetPropertyHeight(valueProperty) : 0f
            select Mathf.Max(keyPropertyHeight, valuePropertyHeight)).Sum();

        var conflictState = GetConflictState(property);

        if (conflictState.conflictIndex != -1)
        {
            propertyHeight += conflictState.conflictLineHeight;
        }

        return propertyHeight;
    }

    private static ConflictState GetConflictState(SerializedProperty property)
    {
        ConflictState conflictState;
        var propId = new PropertyIdentity(property);
        if (s_conflictStateDict.TryGetValue(propId, out conflictState)) return conflictState;
        conflictState = new ConflictState();
        s_conflictStateDict.Add(propId, conflictState);

        return conflictState;
    }

    private static Dictionary<SerializedPropertyType, PropertyInfo> s_serializedPropertyValueAccessorsDict;

    static SerializableDictionaryPropertyDrawer()
    {
        var serializedPropertyValueAccessorsNameDict = new Dictionary<SerializedPropertyType, string>()
        {
            { SerializedPropertyType.Integer, "intValue" },
            { SerializedPropertyType.Boolean, "boolValue" },
            { SerializedPropertyType.Float, "floatValue" },
            { SerializedPropertyType.String, "stringValue" },
            { SerializedPropertyType.Color, "colorValue" },
            { SerializedPropertyType.ObjectReference, "objectReferenceValue" },
            { SerializedPropertyType.LayerMask, "intValue" },
            { SerializedPropertyType.Enum, "intValue" },
            { SerializedPropertyType.Vector2, "vector2Value" },
            { SerializedPropertyType.Vector3, "vector3Value" },
            { SerializedPropertyType.Vector4, "vector4Value" },
            { SerializedPropertyType.Rect, "rectValue" },
            { SerializedPropertyType.ArraySize, "intValue" },
            { SerializedPropertyType.Character, "intValue" },
            { SerializedPropertyType.AnimationCurve, "animationCurveValue" },
            { SerializedPropertyType.Bounds, "boundsValue" },
            { SerializedPropertyType.Quaternion, "quaternionValue" },
        };
        var serializedPropertyType = typeof(SerializedProperty);

        s_serializedPropertyValueAccessorsDict = new Dictionary<SerializedPropertyType, PropertyInfo>();
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

        foreach (var kvp in serializedPropertyValueAccessorsNameDict)
        {
            var propertyInfo = serializedPropertyType.GetProperty(kvp.Value, flags);
            s_serializedPropertyValueAccessorsDict.Add(kvp.Key, propertyInfo);
        }
    }

    private static GUIContent IconContent(string name, string tooltip)
    {
        var builtinIcon = EditorGUIUtility.IconContent(name);
        return new GUIContent(builtinIcon.image, tooltip);
    }

    private static GUIContent TempContent(string text)
    {
        s_tempContent.text = text;
        return s_tempContent;
    }

    private static void DeleteArrayElementAtIndex(SerializedProperty arrayProperty, int index)
    {
        var property = arrayProperty.GetArrayElementAtIndex(index);

        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            property.objectReferenceValue = null;
        }

        arrayProperty.DeleteArrayElementAtIndex(index);
    }

    public static object GetPropertyValue(SerializedProperty p)
    {
        if (s_serializedPropertyValueAccessorsDict.TryGetValue(p.propertyType, out var propertyInfo))
        {
            return propertyInfo.GetValue(p, null);
        }

        return p.isArray
            ? GetPropertyValueArray(p)
            : GetPropertyValueGeneric(p);
    }

    private static void SetPropertyValue(SerializedProperty p, object v)
    {
        PropertyInfo propertyInfo;
        if (s_serializedPropertyValueAccessorsDict.TryGetValue(p.propertyType, out propertyInfo))
        {
            propertyInfo.SetValue(p, v, null);
        }
        else
        {
            if (p.isArray)
                SetPropertyValueArray(p, v);
            else
                SetPropertyValueGeneric(p, v);
        }
    }

    private static object GetPropertyValueArray(SerializedProperty property)
    {
        var array = new object[property.arraySize];
        for (var i = 0; i < property.arraySize; i++)
        {
            var item = property.GetArrayElementAtIndex(i);
            array[i] = GetPropertyValue(item);
        }

        return array;
    }

    private static object GetPropertyValueGeneric(SerializedProperty property)
    {
        var dict = new Dictionary<string, object>();
        var iterator = property.Copy();
        if (!iterator.Next(true)) return dict;
        var end = property.GetEndProperty();
        do
        {
            var name = iterator.name;
            var value = GetPropertyValue(iterator);
            dict.Add(name, value);
        } while (iterator.Next(false) && iterator.propertyPath != end.propertyPath);

        return dict;
    }

    private static void SetPropertyValueArray(SerializedProperty property, object v)
    {
        var array = (object[])v;
        property.arraySize = array.Length;
        for (var i = 0; i < property.arraySize; i++)
        {
            var item = property.GetArrayElementAtIndex(i);
            SetPropertyValue(item, array[i]);
        }
    }

    private static void SetPropertyValueGeneric(SerializedProperty property, object v)
    {
        var dict = (Dictionary<string, object>)v;
        var iterator = property.Copy();
        if (!iterator.Next(true)) return;

        var end = property.GetEndProperty();
        do
        {
            var name = iterator.name;
            SetPropertyValue(iterator, dict[name]);
        } while (iterator.Next(false) && iterator.propertyPath != end.propertyPath);
    }

    private static bool ComparePropertyValues(object value1, object value2)
    {
        if (value1 is Dictionary<string, object> dict1 && value2 is Dictionary<string, object> dict2)
        {
            return CompareDictionaries(dict1, dict2);
        }

        return Equals(value1, value2);
    }

    private static bool CompareDictionaries(Dictionary<string, object> dict1, Dictionary<string, object> dict2)
    {
        if (dict1.Count != dict2.Count)
            return false;

        foreach (var (key1, value1) in dict1)
        {
            if (!dict2.TryGetValue(key1, out var value2))
                return false;

            if (!ComparePropertyValues(value1, value2))
                return false;
        }

        return true;
    }

    private struct EnumerationEntry
    {
        public SerializedProperty keyProperty;
        public SerializedProperty valueProperty;
        public int index;

        public EnumerationEntry(SerializedProperty keyProperty, SerializedProperty valueProperty, int index)
        {
            this.keyProperty = keyProperty;
            this.valueProperty = valueProperty;
            this.index = index;
        }
    }

    private static IEnumerable<EnumerationEntry> EnumerateEntries(SerializedProperty keyArrayProperty,
        SerializedProperty valueArrayProperty, int startIndex = 0)
    {
        if (keyArrayProperty.arraySize <= startIndex) yield break;

        var index = startIndex;
        var keyProperty = keyArrayProperty.GetArrayElementAtIndex(startIndex);
        var valueProperty = valueArrayProperty?.GetArrayElementAtIndex(startIndex);
        var endProperty = keyArrayProperty.GetEndProperty();

        do
        {
            yield return new EnumerationEntry(keyProperty, valueProperty, index);
            index++;
        } while (keyProperty.Next(false)
                 && (valueProperty?.Next(false) ?? true)
                 && !SerializedProperty.EqualContents(keyProperty, endProperty));
    }
}