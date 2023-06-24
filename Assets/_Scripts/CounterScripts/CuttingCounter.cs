using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    public override void Interact(Player_TopDown player)
    {
        if (!HasKitchenObject())
        {
            // there is no KitchenObject on the counter
            if (player.HasKitchenObject())
            {
                // player is carrying something
                // then move the KitchenObject from player's hand to this Counter.
                player.GetKitchenObject().SetKitchenObjectParent(this);
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
                // player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }


    public override void InteractAlternate(Player_TopDown player)
    {
        if (HasKitchenObject())
        {
            // there is a kitchen object here
            // perform cutting operation

            // destroy the current object
            GetKitchenObject().DestroySelf();

            // spawn the cut KitchenObject
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }


    }
}
