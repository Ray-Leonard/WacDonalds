using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter 
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    // the time interval between each spawn
    private float spawnPlateTimer = 0f;
    [SerializeField] private float spawnPlateTimerMax = 4f;

    // keep track of how many plates has been spawned.
    private int platesSpawnedAmount = 0;
    [SerializeField] private int platesSpawnedAmountMax = 4;


    // logic of updating the available plate
    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if(spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;

            if(platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player_TopDown player)
    {
        // there is at least 1 plate to pick up
        if (platesSpawnedAmount > 0)
        {
            // player is not carrying anything
            if (!player.HasKitchenObject())
            {
                // decrease the counter amount
                platesSpawnedAmount--;

                // spawn the actual plate kitchen object and give it to player
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                // update the visuals.
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
