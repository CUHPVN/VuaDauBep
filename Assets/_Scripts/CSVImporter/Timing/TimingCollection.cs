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
public class TimingData
{
	public int Level;
	public float Green;
	public float Yellow;
}

[CreateAssetMenu(fileName = "TimingCollection", menuName = "Scriptable Object/Timing Collection")]
public class TimingCollection : ScriptableObject
{
	// This path points to the CSV Collection Manager Scriptable Object. 
	private const string CSVCollectionPath = "Assets/ScriptableObject/CSVCollectionManager/CSVCollectionManager.asset";

	public List<TimingData> DataTable;

	/// <summary>
	/// This function will read the data from the CSV file, and populate the DataTable. This should be done in editor time to update the Localization Data
	/// each time you update the CSV data.
	/// </summary>
	public void PopulateDataFromCSV()
	{
#if UNITY_EDITOR
        CSVCollectionManager collectionManager = AssetDatabase.LoadAssetAtPath<CSVCollectionManager>(CSVCollectionPath);

        if (collectionManager != null)
        {
            TextAsset textAsset = collectionManager.GetCollection(nameof(TimingData));
            if (textAsset == null)
            {
                Debug.LogError($"Text Asset collection with name: {nameof(TimingData)} doesn't exist.");
                return;
            }

            DataTable.Clear();
            DataTable.AddRange(CSVImporter.Parse<TimingData>(textAsset.text));

            AssetDatabase.SaveAssets();
        }
#endif
    }
}