using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;
    public new static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player_TopDown player)
    {
        // if the player is holding something, destroy it
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();

            OnAnyObjectTrashed.Invoke(this, EventArgs.Empty);
        }
    }
}
