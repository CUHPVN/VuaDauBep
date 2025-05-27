using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipeSO", menuName = "Scriptable Object/CuttingRecipeSO")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
}
