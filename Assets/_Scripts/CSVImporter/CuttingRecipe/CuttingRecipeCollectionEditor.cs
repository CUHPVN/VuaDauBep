using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CuttingRecipeCollection))]
public class CuttingRecipeCollectionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Populate Data From CSV"))
		{
			var cuttingRecipeCollection = (CuttingRecipeCollection)target;
			cuttingRecipeCollection.PopulateDataFromCSV();
		}
	}
}
#endif