using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    private IKitchenObjectParent kitchentObjectParent;
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }
    public void SetKitchenObjectParent(IKitchenObjectParent clearCounter)
    {
        if(clearCounter.HasKitchenObject())
        {
            Debug.LogError("KitchenObjectParent Has Kitchen Object!");
            return;
        }
        if(this.kitchentObjectParent != null)
        {
            this.kitchentObjectParent.ClearKitchenObject();
        }
        this.kitchentObjectParent = clearCounter;
        clearCounter.SetKitchenObject(this);
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return this.kitchentObjectParent;
    }
}
