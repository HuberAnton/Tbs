using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// This class is mostly not needed since you
// can create buttons using attributes.
[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor
{
    public BoardCreator current
    {
        get
        {
            return (BoardCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Clear"))
            current.Clear();
        if (GUILayout.Button("Grow"))
            current.Grow();
        if (GUILayout.Button("Shrink"))
            current.Shrink();
        if (GUILayout.Button("Grow Area"))
            current.GrowArea();
        if (GUILayout.Button("Shrink Area"))
            current.ShrinkArea();
        if (GUILayout.Button("Save"))
            current.Save();
        if (GUILayout.Button("Load"))
            current.Load();
        if (GUILayout.Button("Fill"))
            current.Fill();
        if (GUI.changed)
            current.UpdateMarker();
    }

}
