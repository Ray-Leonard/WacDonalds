using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
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
            if(player.HasKitchenObject())
            {
                // player is carrying something, then check if player is holding a plate
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObjectPlayer))
                {
                    // give the kitchen object on the counter to player's plate.
                    if (plateKitchenObjectPlayer.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // destroy the KO on the counter.
                        GetKitchenObject().DestroySelf();
                    }
                }
                // player is not holding a plate, but something else
                else
                {
                    // then check if there's a plate on the counter
                    if(GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObjectCounter))
                    {
                        // try to add whatever player's carrying onto the plate
                        if (plateKitchenObjectCounter.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) 
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }

                }
            }
            else
            {
                // player is not carrying anything, give the kitchen object to player. 
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
