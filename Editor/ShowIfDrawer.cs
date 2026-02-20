using System;

using UnityEngine;
using UnityEditor;

namespace MagmaLabs.Editor
{

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldShow(property) 
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
                EditorGUI.PropertyField(position, property, label, true);
        }

        virtual protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);

            if (condition == null)
            {
                Debug.LogError($"ShowIf: Field '{showIf.conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(condition);
            return conditionValue.Equals(showIf.compareValue);
        }

        protected object GetSerializedPropertyValue(SerializedProperty property)
        {
            return property.propertyType switch
            {
                SerializedPropertyType.Boolean => property.boolValue,
                SerializedPropertyType.Integer => property.intValue,
                SerializedPropertyType.Float => property.floatValue,
                SerializedPropertyType.String => property.stringValue,
                _ => null,
            };
        }


        }
    

[CustomPropertyDrawer(typeof(ShowIfNotAttribute))]
public class ShowIfNotDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            return !base.ShouldShow(property);
        }
    }   
[CustomPropertyDrawer(typeof(ShowIfGreaterThanAttribute))]
public class ShowIfGreaterThanDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);

            if (condition == null)
            {
                Debug.LogError($"ShowIfGreaterThan: Field '{showIf.conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue is IComparable comparableCondition && showIf.compareValue is IComparable comparableCompare)
            {
                return comparableCondition.CompareTo(comparableCompare) > 0;
            }
            else
            {
                Debug.LogError($"ShowIfGreaterThan: Field '{showIf.conditionField}' or compare value is not comparable.");
                return true;
            }
        }
    }

[CustomPropertyDrawer(typeof(ShowIfLessThanAttribute))]
public class ShowIfLessThanDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);

            if (condition == null)
            {
                Debug.LogError($"ShowIfLessThan: Field '{showIf.conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue is IComparable comparableCondition && showIf.compareValue is IComparable comparableCompare)
            {
                return comparableCondition.CompareTo(comparableCompare) < 0;
            }
            else
            {
                Debug.LogError($"ShowIfLessThan: Field '{showIf.conditionField}' or compare value is not comparable.");
                return true;
            }
        }
    }

[CustomPropertyDrawer(typeof(ShowIfGreaterThanOrEqualAttribute))]
public class ShowIfGreaterThanOrEqualDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);

            if (condition == null)
            {
                Debug.LogError($"ShowIfGreaterEqual: Field '{showIf.conditionField}' not found.");
                return true;
            }

            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue is IComparable comparableCondition && showIf.compareValue is IComparable comparableCompare)
            {
                return comparableCondition.CompareTo(comparableCompare) >= 0;
            }
            else
            {
                Debug.LogError($"ShowIfGreaterEqual: Field '{showIf.conditionField}' or compare value is not comparable.");
                return true;
            }
        }
    }

[CustomPropertyDrawer(typeof(ShowIfLessThanOrEqualAttribute))]

public class ShowIfLessThanOrEqualDrawer : ShowIfDrawer
    {
        override protected bool ShouldShow(SerializedProperty property)
        {
            var showIf = (ShowIfAttribute)attribute;

            SerializedProperty condition =
                property.serializedObject.FindProperty(showIf.conditionField);

            if (condition == null)
            {
                Debug.LogError($"ShowIfLessEqual: Field '{showIf.conditionField}' not found.");
                return true;
            }
        
            object conditionValue = GetSerializedPropertyValue(condition);
            if (conditionValue is IComparable comparableCondition && showIf.compareValue is IComparable comparableCompare)
            {
                return comparableCondition.CompareTo(comparableCompare) <= 0;
            }
            else
            {
                Debug.LogError($"ShowIfLessEqual: Field '{showIf.conditionField}' or compare value is not comparable.");
                return true;
            }
        }
    }
}