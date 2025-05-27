using UnityEngine;

[CreateAssetMenu(fileName = "BurningRecipeSO", menuName = "Scriptable Object/BurningRecipeSO")]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float burningTimerMax;
}
