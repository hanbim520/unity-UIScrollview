using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnityEngine.UI.UIScrollView))]
public class ScrollViewEditor : UnityEditor.Editor
{
    private SerializedObject serialize;//序列化
    private SerializedProperty alignment, countOfRow, countOfCol, cellWidthSpace, cellHeightSpace, scrollRect, content, cellAlignMent;

    void OnEnable()
    {
        serialize = new SerializedObject(target);
        alignment = serialize.FindProperty("alignment");
        countOfRow = serialize.FindProperty("CountOfRow");
        countOfCol = serialize.FindProperty("CountOfCol");
        cellWidthSpace = serialize.FindProperty("HorizontalSpace");
        cellHeightSpace = serialize.FindProperty("VerticalSpace");
        scrollRect = serialize.FindProperty("m_ScrollRect");
        content = serialize.FindProperty("m_Content");
        cellAlignMent = serialize.FindProperty("cellAlignMent");
    }
    public override void OnInspectorGUI()
    {
        if (serialize == null) return;
        serialize.Update();
        EditorGUILayout.PropertyField(alignment);
        
        if (alignment.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(countOfRow);
        }
        else if (alignment.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(countOfCol);
        }
        EditorGUILayout.PropertyField(cellWidthSpace);
        EditorGUILayout.PropertyField(cellHeightSpace);
        EditorGUILayout.PropertyField(scrollRect);
        EditorGUILayout.PropertyField(content);
        EditorGUILayout.PropertyField(cellAlignMent);
        serialize.ApplyModifiedProperties();//应用
    }
}
