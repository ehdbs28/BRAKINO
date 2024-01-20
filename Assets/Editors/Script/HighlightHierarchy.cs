#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HighlightHierarchy : MonoBehaviour
{
    private static readonly Dictionary<Object, HighlightHierarchy> _coloredObjects;

    static HighlightHierarchy()
    {
        _coloredObjects = new Dictionary<Object, HighlightHierarchy>();
        EditorApplication.hierarchyWindowItemOnGUI += HandleDraw;
    }

    private static void HandleDraw(int instanceId, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceId);

        if(obj != null && _coloredObjects.ContainsKey(obj))
        {
            var gObj = obj as GameObject;
            var ch = gObj.GetComponent<HighlightHierarchy>();
            if(ch != null)
            {
                PaintObject(obj, selectionRect, ch);
            }
            else
            {
                _coloredObjects.Remove(obj);
            }
        }
            
    }

    private static void PaintObject(Object obj, Rect selectionRect, HighlightHierarchy ch)
    {
        var bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

        if (Selection.activeObject != obj)
        {
            EditorGUI.DrawRect(bgRect, ch.backColor);

            string name = $"{ch.prefix}  {obj.name}";

            EditorGUI.LabelField(bgRect, name, new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = ch.fontColor },
                fontStyle = FontStyle.Bold
            });
        }
    }

    public string prefix;
    public Color backColor;
    public Color fontColor;

    private void Reset()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        _coloredObjects.TryAdd(this.gameObject, this);
    }
}

#endif