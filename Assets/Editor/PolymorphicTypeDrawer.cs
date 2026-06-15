using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public abstract class PolymorphicTypeDrawer<T> : PropertyDrawer
{
    private static Type[] types;
    private static string[] typeNames;

    private void buildTypeList()
    {
        if (types != null) return;

        List<Type> foundTypes = new List<Type>();
        TypeCache.TypeCollection derivedTypes = TypeCache.GetTypesDerivedFrom<T>();

        foreach (Type type in derivedTypes)
        {
            if (!type.IsAbstract)
                foundTypes.Add(type);
        }

        foundTypes.Sort(compareTypeNames);

        types = foundTypes.ToArray();
        typeNames = new string[types.Length];

        for (int i = 0; i < types.Length; i++)
            typeNames[i] = types[i].Name;
    }

    private static int compareTypeNames(Type a, Type b)
    {
        return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        buildTypeList();

        Type currentType = property.managedReferenceValue?.GetType();
        int selectedIndex = -1;

        for (int i = 0; i < types.Length; i++)
        {
            if (types[i] == currentType)
            {
                selectedIndex = i;
                break;
            }
        }

        Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        int newIndex = EditorGUI.Popup(rect, label.text, selectedIndex, typeNames);

        if (newIndex != selectedIndex)
        {
            property.managedReferenceValue = Activator.CreateInstance(types[newIndex]);
            property.serializedObject.ApplyModifiedProperties();
        }

        if (property.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            Rect childRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, position.height - EditorGUIUtility.singleLineHeight - 2);
            EditorGUI.PropertyField(childRect, property, GUIContent.none, true);
            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.managedReferenceValue != null)
            return EditorGUI.GetPropertyHeight(property, true) + EditorGUIUtility.singleLineHeight + 2;

        return EditorGUIUtility.singleLineHeight;
    }
}
