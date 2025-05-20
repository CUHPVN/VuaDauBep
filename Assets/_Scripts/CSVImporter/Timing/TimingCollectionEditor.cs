using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TimingCollection))]
public class TimingCollectionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Populate Data From CSV"))
		{
			var timingCollection = (TimingCollection)target;
			timingCollection.PopulateDataFromCSV();
		}
	}
}
#endif