using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

[CustomEditor(typeof(vLayersAndTags))]
public class vLauersAndTagsEditor : Editor
{
    public ReorderableList layerList;
    public ReorderableList tagList;
    public GUISkin skin;
    private void OnEnable()
    {
        skin = Resources.Load("skin") as GUISkin;
      
        layerList = new ReorderableList(serializedObject, serializedObject.FindProperty("layers"), true, true, true, true);
        tagList = new ReorderableList(serializedObject, serializedObject.FindProperty("tags"), true, true, true, true);
        layerList.drawElementCallback = DrawLayers;
      
       
       
        layerList.drawHeaderCallback = (rect) =>
        {
            var _rect = rect;
            _rect.width = rect.width * 0.5f;
            GUI.Label(_rect, "Layers");
            _rect.width = rect.width * 0.4f;
           
            _rect.x =(rect.x+ rect.width) - _rect.width;
            _rect.y += 2;
            _rect.height -= 4;
            if (GUI.Button(_rect, "Clear", EditorStyles.miniButton))
            {
                layerList.serializedProperty.ClearArray();
            }
        };

       
        tagList.drawElementCallback = DrawTags;
          
        tagList.drawHeaderCallback = (rect) =>
        {
            var _rect = rect;
            _rect.width = rect.width * 0.5f;
            GUI.Label(_rect, "Tags");
            _rect.width = rect.width * 0.4f;
            _rect.x = (rect.x + rect.width) - (_rect.width);
            _rect.y += 2;
            _rect.height -= 4;
            if (GUI.Button(_rect, "Clear", EditorStyles.miniButton))
            {
                tagList.serializedProperty.ClearArray();
            }
        };
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.BeginVertical("INVECTOR LAYERS AND TAGS",skin.window);
        {
            GUILayout.Space(30);
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    layerList.DoLayoutList();
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                {
                    tagList.DoLayoutList();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
    private void DrawLayers(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, layerList.serializedProperty.GetArrayElementAtIndex(index),GUIContent.none);
    }
    private void DrawTags(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.PropertyField(rect, tagList.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
    }
}
