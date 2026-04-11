using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(BaseStatusEffect), true)]
public class BaseStatusEffectDrawer : PropertyDrawer
{
    private Type[] types;
    private string[] typeNames;

    public BaseStatusEffectDrawer()
    {
        types = new Type[]
        {
            typeof(PoisonEffect),
            typeof(BleedingEffect),
            typeof(RegenerationEffect),
            typeof(StrengthEffect),
            typeof(ThornsEffect),
            typeof(VulnerableEffect)
        };

        typeNames = new string[types.Length];
        for (int i = 0; i < types.Length; i++)
            typeNames[i] = types[i].Name;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int selectedIndex = -1;
        Type currentType = property.managedReferenceValue?.GetType();

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