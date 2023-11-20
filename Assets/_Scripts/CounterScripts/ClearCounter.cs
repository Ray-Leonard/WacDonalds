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
                // player is carrying something, then do nothing
            }
            else
            {
                // player is not carrying anything, give the kitchen object to player. 
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
