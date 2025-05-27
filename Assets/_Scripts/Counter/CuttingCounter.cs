using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private List<CuttingRecipeSO> cuttingRecipeSOList;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject()&& HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject()&&HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            KitchenObjectSO kitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(kitchenObject,this);
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        return GetCuttingRecipeSOWithInput(input) != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        return GetCuttingRecipeSOWithInput(input).output;
    }
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO input)
    {
        foreach (var cuttingRecipeSO in cuttingRecipeSOList)
        {
            if (cuttingRecipeSO.input == input) return cuttingRecipeSO;
        }
        return null;
    }
}
