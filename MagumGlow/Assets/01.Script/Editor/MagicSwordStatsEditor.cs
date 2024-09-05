// Assets/Editor/MagicSwordStatsEditor.cs
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MagicSwordStats))]
public class MagicSwordStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MagicSwordStats magicSwordStats = (MagicSwordStats)target;

        if (GUILayout.Button("Reset Stats"))
        {
            magicSwordStats.ResetStats();
        }
    }
}
