using UnityEngine;

[CreateAssetMenu(fileName = "FryingRecipeSO", menuName = "Scriptable Object/FryingRecipeSO")]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fryingTimerMax;
}
