#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.Presets;
using System.Collections.Generic;
using System.IO;

public static class CuttingRecipeGenerator
{
    private const string collectionPath = "Assets/Resources/CuttingRecipeCollection.asset";
    private const string outputFolderPath = "Assets/ScriptableObject/CuttingRecipeSO";
    private const string cuttingCounterPrefabFolder = "Assets/Prefabs/Counters"; // 🔧 Thay đổi nếu prefab bạn ở folder khác

    [MenuItem("Tools/Generate CuttingRecipeSO And Assign To Prefab")]
    public static void GenerateCuttingRecipes()
    {
        if (!Directory.Exists(outputFolderPath))
            Directory.CreateDirectory(outputFolderPath);

        var collection = AssetDatabase.LoadAssetAtPath<CuttingRecipeCollection>(collectionPath);
        if (collection == null)
        {
            Debug.LogError("❌ CuttingRecipeCollection not found!");
            return;
        }

        var createdRecipes = new List<CuttingRecipeSO>();

        foreach (var data in collection.DataTable)
        {
            var inputSO = FindKitchenObjectSO(data.input);
            var outputSO = FindKitchenObjectSO(data.output);

            if (inputSO == null || outputSO == null)
            {
                Debug.LogWarning($"⚠️ Missing KitchenObjectSO: id={data.id}, input={data.input}, output={data.output}");
                continue;
            }

            var asset = ScriptableObject.CreateInstance<CuttingRecipeSO>();
            asset.input = inputSO;
            asset.output = outputSO;

            string fileName = $"{data.input}-{data.output}";
            string fullPath = $"{outputFolderPath}/{fileName}.asset";

            AssetDatabase.CreateAsset(asset, fullPath);
            createdRecipes.Add(asset);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        AssignRecipesToCuttingCounterPrefabs(createdRecipes);

        Debug.Log($"✅ Generated {createdRecipes.Count} CuttingRecipeSO and assigned to CuttingCounter prefab(s).");
    }

    private static KitchenObjectSO FindKitchenObjectSO(string name)
    {
        string[] guids = AssetDatabase.FindAssets($"t:KitchenObjectSO {name}");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var so = AssetDatabase.LoadAssetAtPath<KitchenObjectSO>(path);
            if (so != null && so.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                return so;
        }
        return null;
    }

    private static void AssignRecipesToCuttingCounterPrefabs(List<CuttingRecipeSO> recipes)
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { cuttingCounterPrefabFolder });

        int assignedCount = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            var counterInPrefab = prefab.GetComponent<CuttingCounter>();
            if (counterInPrefab == null) continue;

            var prefabRoot = PrefabUtility.LoadPrefabContents(path);
            var counter = prefabRoot.GetComponent<CuttingCounter>();
            if (counter == null)
            {
                PrefabUtility.UnloadPrefabContents(prefabRoot);
                continue;
            }

            Undo.RecordObject(counter, "Assign CuttingRecipeSO List");

            var field = typeof(CuttingCounter).GetField("cuttingRecipeSOList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(counter, recipes);

            EditorUtility.SetDirty(counter);
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            PrefabUtility.UnloadPrefabContents(prefabRoot);

            assignedCount++;
        }

        if (assignedCount == 0)
            Debug.LogWarning("⚠️ No CuttingCounter prefab found to assign.");
    }
}
#endif
