using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContainerCounter : BaseCounter
{
    // event to trigger when play grabs an object from container, notify the container visual to play animation.
    public event EventHandler OnPlayerGrabObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player_TopDown player)
    {
        if(!player.HasKitchenObject())
        {
            // spawn the kitchen object when the player is not carrying anything.
            // give it to the player immediately after spawning.
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
        }
    }


}
