/**
 * ClassName: ScrollViewEditor.cs
 * 
 * @author NavyZhang
 * @date 2018-01-30 上午11:15:43
 * @version 1.0
 * 
 * 
 * ............................................
 *                       _oo0oo_
 *                      o8888888o
 *                      88" . "88
 *                      (| -_- |)
 *                      0\  =  /0
 *                    ___/`---'\___
 *                  .' \\|     |// '.
 *                 / \\|||  :  |||// \
 *                / _||||| -卍-|||||- \
 *               |   | \\\  -  /// |   |
 *               | \_|  ''\---/''  |_/ |
 *               \  .-\__  '-'  ___/-. /
 *             ___'. .'  /--.--\  `. .'___
 *          ."" '<  `.___\_<|>_/___.' >' "".
 *         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
 *         \  \ `_.   \_ __\ /__ _/   .-` /  /
 *     =====`-.____`.___ \_____/___.-`___.-'=====
 *                       `=---='
 *                       
 *..................佛祖开光 ,永无BUG...................
 * 
 */
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
