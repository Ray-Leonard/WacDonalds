using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    // if cutting is in progress, then player can't pick up the im progress item.
    private bool canPickup;

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
    public event EventHandler OnCut;

    public override void Interact(Player_TopDown player)
    {
        if (!HasKitchenObject())
        {
            // there is no KitchenObject on the counter
            if (player.HasKitchenObject())
            {
                // player is carrying something

                // check if player is holding something that can be cut
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // then move the KitchenObject from player's hand to this Counter.
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    // reset cutting progress when player put something on the counter
                    cuttingProgress = 0;

                    // when put something on the counter first, allow pickup.
                    canPickup = true;

                    // fire off the event to update the progress
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
                        progressNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
            else
            {
                // player is not carrying anything
            }
        }
        else
        {
            // there IS KitchenObject on the counter
            if (player.HasKitchenObject())
            {
                // player is carrying something
            }
            else
            {
                // player is not carrying anything, transfer the object on player's hand.
                if(canPickup)
                { 
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }
    }


    public override void InteractAlternate(Player_TopDown player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            // there is a kitchen object here && it can be cut
            // perform cutting operation

            // lock so don't allow pickup the object
            canPickup = false;

            // fire off OnCut event
            OnCut?.Invoke(this, EventArgs.Empty);

            // compare the cutting progress with the max defined in the recipe
            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

            // fire off event to update progress
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs{
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });


            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                // spawn the output object:
                // keep an reference to the Output KitchenObjectSO
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                // destroy the current object
                GetKitchenObject().DestroySelf();

                // spawn the cut KitchenObject
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);

                // cutting finished, unlock to allow pickup
                canPickup = true;
            }


        }
    }


    private CuttingRecipeSO GetCuttingRecipeSOForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }
        return null;
    }



    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(inputKitchenObjectSO);

        if(cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }

        return null;
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOForInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }




}
