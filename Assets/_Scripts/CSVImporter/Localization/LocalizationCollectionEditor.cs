using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(LocalizationCollection))]
public class LocalizationCollectionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Populate Data From CSV"))
		{
			var localizationCollection = (LocalizationCollection)target;
			localizationCollection.PopulateDataFromCSV();
		}
	}
}
#endif