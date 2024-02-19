using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player_TopDown player)
    {
        if (player.HasKitchenObject())
        {
            // delivery counter will only accept plates.
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.DeliverOrder(plateKitchenObject);

                // destroy the plate at player's hand (work the same as a trach counter)
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}
