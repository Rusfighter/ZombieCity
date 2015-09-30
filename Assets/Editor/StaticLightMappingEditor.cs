using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticLightMapping))]
public class StaticLightMappingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StaticLightMapping myScript = (StaticLightMapping)target;
        if (GUILayout.Button("Set Lightmap Static"))
        {
            myScript.SetLightMapStatic();
        }
    }
}
