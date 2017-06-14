using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GenerateLevel))]
public class LevelGenEditor : Editor {

    public override void OnInspectorGUI()
    {
        GenerateLevel mapGen = (GenerateLevel)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenMap();
        }
    }
}
