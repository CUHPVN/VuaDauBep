using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public CookingState cookingState;
    }
    [SerializeField] private List<FryingRecipeSO> fryingRecipeSOList;
    [SerializeField] private List<BurningRecipeSO> burningRecipeSOList;

    private float fryingTimer=0;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer = 0;
    private BurningRecipeSO burningRecipeSO;
    public enum CookingState
    {
        Idle,
        Cooking,
        Cooked,
        Burned,
    }
    private CookingState cookingState=CookingState.Idle;
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (cookingState)
            {
                case CookingState.Idle:
                    break;
                case CookingState.Cooking:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
                        color = Color.green
                    });
                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                    {
                        fryingTimer = 0;
                        cookingState = CookingState.Cooked;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        burningTimer = 0;
                        burningRecipeSO = GetBurningRecipeSOWithInput(fryingRecipeSO.output);
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            cookingState = cookingState,
                        });
                    }
                    break;
                case CookingState.Cooked:
                    burningTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax,
                        color = Color.red
                    });
                    if (burningTimer >= burningRecipeSO.burningTimerMax)
                    {
                        burningTimer = 0;
                        cookingState = CookingState.Burned;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            cookingState = cookingState,
                        });
                        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                        {
                            progressNormalized = 1,
                            color = Color.black
                        });
                    }
                    break;
                case CookingState.Burned:
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        progressNormalized = 1,
                        color = Color.black
                    });
                    break;
            }
            
        }
    }
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                cookingState = CookingState.Cooking;
                fryingTimer = 0;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    cookingState = cookingState,
                });
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax,
                    color = Color.green
                });
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                cookingState = CookingState.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    cookingState = cookingState,
                });
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    progressNormalized = 0,
                    color = Color.white
                });
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            KitchenObjectSO kitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(kitchenObject, this);
        }
    }
    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        return GetFryingRecipeSOWithInput(input) != null;
    }
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        return GetFryingRecipeSOWithInput(input).output;
    }
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO input)
    {
        foreach (var fryingRecipeSO in fryingRecipeSOList)
        {
            if (fryingRecipeSO.input == input) return fryingRecipeSO;
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO input)
    {
        foreach (var burningRecipeSO in burningRecipeSOList)
        {
            if (burningRecipeSO.input == input) return burningRecipeSO;
        }
        return null;
    }
}
