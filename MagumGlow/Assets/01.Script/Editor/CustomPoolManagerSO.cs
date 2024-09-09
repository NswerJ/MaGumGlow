using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(PoolManagerSO))]
public class CustomPoolManagerSO : Editor
{
    private PoolManagerSO _manager;

    private void OnEnable()
    {
        _manager = target as PoolManagerSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Get all pool item"))
        {
            UpdatePoolingItems();
        }
    }

    private void UpdatePoolingItems()
    {
        List<PoolingItemSO> loadedItems = new List<PoolingItemSO>();

        string[] assetGUIDs = AssetDatabase.FindAssets("", new[] { "Assets/07.SO/Pool/Items" });

        foreach (string guid in assetGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            PoolingItemSO item = AssetDatabase.LoadAssetAtPath<PoolingItemSO>(assetPath);

            if (item != null)
                loadedItems.Add(item);
        }

        _manager.poolingItems = loadedItems;

        EditorUtility.SetDirty(_manager);
        AssetDatabase.SaveAssets();
    }
}
