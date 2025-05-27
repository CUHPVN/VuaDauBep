using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class represent each data row within the CSV.
/// Each variable name should be EXACTLY the same on the first row of the CSV file. The order can be changed, but for clarity, it's best to keep the same order.
/// </summary>
[Serializable]
public class CuttingRecipeData
{
	public int id;
	public string input;
	public string output;
}

[CreateAssetMenu(fileName = "CuttingRecipeCollection", menuName = "Scriptable Object/CuttingRecipe Collection")]
public class CuttingRecipeCollection : ScriptableObject
{
	// This path points to the CSV Collection Manager Scriptable Object. 
	private const string CSVCollectionPath = "Assets/ScriptableObject/CSVCollectionManager/CSVCollectionManager.asset";

	public List<CuttingRecipeData> DataTable;

    /// <summary>
    /// This function will read the data from the CSV file, and populate the DataTable. This should be done in editor time to update the Localization Data
    /// each time you update the CSV data.
    /// </summary>
#if UNITY_EDITOR
    public void PopulateDataFromCSV()
	{
        CSVCollectionManager collectionManager = AssetDatabase.LoadAssetAtPath<CSVCollectionManager>(CSVCollectionPath);

        if (collectionManager != null)
        {
            TextAsset textAsset = collectionManager.GetCollection(nameof(CuttingRecipeData));
            if (textAsset == null)
            {
                Debug.LogError($"Text Asset collection with name: {nameof(CuttingRecipeData)} doesn't exist.");
                return;
            }

            DataTable.Clear();
            DataTable.AddRange(CSVImporter.Parse<CuttingRecipeData>(textAsset.text));

            AssetDatabase.SaveAssets();
        }
    }
#endif
}