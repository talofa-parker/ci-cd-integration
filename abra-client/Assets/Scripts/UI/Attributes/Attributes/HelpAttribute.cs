﻿using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TalofaGames.UI.Attributes
{
  [AttributeUsage(AttributeTargets.Field)]
  public class HelpAttribute : PropertyAttribute
  {
    public string Text { get; }
    public HelpType Type { get; }

    public HelpAttribute(string text, HelpType type = HelpType.Info)
    {
      Text = text;
      Type = type;
    }
  }

  public enum HelpType
  {
    Info,
    Warning,
    Error
  }

#if UNITY_EDITOR

  [CustomPropertyDrawer(typeof(HelpAttribute))]
  public class HelpAttributeDrawer : PropertyDrawer
  {
    private const int paddingHeight = 8;
    private const int marginHeight = 2;
    private float baseHeight = 0;
    private float addedHeight = 0;

    private HelpAttribute HelpAttribute => (HelpAttribute)attribute;

    private RangeAttribute GetRangeAttribute()
    {
      var attributes = fieldInfo.GetCustomAttributes(typeof(RangeAttribute), true);
      return (attributes.Length > 0) ? (RangeAttribute)attributes[0] : null;
    }

    private MultilineAttribute GetMultilineAttribute()
    {
      var attributes = fieldInfo.GetCustomAttributes(typeof(MultilineAttribute), true);
      return (attributes.Length > 0) ? (MultilineAttribute)attributes[0] : null;
    }

    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
    {
      baseHeight = base.GetPropertyHeight(prop, label);

      float minHeight = paddingHeight * 5;

      var content = new GUIContent(HelpAttribute.Text);
      var style = (GUIStyle)"helpbox";

      var height = style.CalcHeight(content, EditorGUIUtility.currentViewWidth);

      height += marginHeight * 2;

      var multilineAttribute = GetMultilineAttribute();
      if (multilineAttribute != null && prop.propertyType == SerializedPropertyType.String)
      {
        addedHeight = 48f;
      }

      return (height > minHeight) ? height + baseHeight + addedHeight : minHeight + baseHeight + addedHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, prop);

      var helpPos = position;
      helpPos.height -= baseHeight + marginHeight;

      var multiline = GetMultilineAttribute();
      if (multiline != null)
      {
        helpPos.height -= addedHeight;
      }

      EditorGUI.HelpBox(helpPos, HelpAttribute.Text, HelpAttribute.Type.GetMessageType());

      position.y += helpPos.height + marginHeight;
      position.height = baseHeight;

      var range = GetRangeAttribute();
      if (range != null)
      {
        if (prop.propertyType == SerializedPropertyType.Float)
        {
          EditorGUI.Slider(position, prop, range.min, range.max, label);
        }
        else if (prop.propertyType == SerializedPropertyType.Integer)
        {
          EditorGUI.IntSlider(position, prop, (int)range.min, (int)range.max, label);
        }
        else
        {
          EditorGUI.PropertyField(position, prop, label);
        }
      }
      else if (multiline != null)
      {
        if (prop.propertyType == SerializedPropertyType.String)
        {
          var style = EditorStyles.label;
          var size = style.CalcHeight(label, EditorGUIUtility.currentViewWidth);

          EditorGUI.LabelField(position, label);

          position.y += size;
          position.height += addedHeight - size;

          prop.stringValue = EditorGUI.TextArea(position, prop.stringValue);
        }
        else
        {
          EditorGUI.PropertyField(position, prop, label);
        }
      }
      else
      {
        EditorGUI.PropertyField(position, prop, label);
      }

      EditorGUI.EndProperty();
    }
  }

  public static class HelpAttributeTypeExtensions
  {
    public static MessageType GetMessageType(this HelpType helpType)
    {
      switch (helpType)
      {
        case HelpType.Info:
          return MessageType.Info;
        case HelpType.Warning:
          return MessageType.Warning;
        case HelpType.Error:
          return MessageType.Error;
        default:
          return MessageType.None;
      }
    }
  }

#endif

}