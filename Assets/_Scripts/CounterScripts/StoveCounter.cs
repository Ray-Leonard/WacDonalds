using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;        // called when timer is modified
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs: EventArgs
    {
        public State _state;
    }


    public enum State
    {
        Idle,
        Frying,
        Cooked,
        Burned
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state = State.Idle;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO currFryingRecipeSO;
    private BurningRecipeSO currBurningRecipeSO;


    private void Update()
    {


        switch (state)
        {
            case State.Idle:
                break;

            case State.Frying:
                fryingTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = fryingTimer / currFryingRecipeSO.fryingTimerMax });
                // object is cooked.
                if (fryingTimer > currFryingRecipeSO.fryingTimerMax)
                {
                    // delete the uncooked object
                    GetKitchenObject().DestroySelf();
                    // spawn the cooked object
                    KitchenObject.SpawnKitchenObject(currFryingRecipeSO.output, this);

                    // advance to the next state, reset next state timer.
                    state = State.Cooked;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });
                    
                    burningTimer = 0f;
                    // get the new recipe
                    currBurningRecipeSO = GetBurningRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());
                }
                break;

            case State.Cooked:
                burningTimer += Time.deltaTime;
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = burningTimer / currBurningRecipeSO.burningTimerMax });
                // object is cooked.
                if (burningTimer > currBurningRecipeSO.burningTimerMax)
                {
                    // delete the uncooked object
                    GetKitchenObject().DestroySelf();
                    // spawn the cooked object
                    KitchenObject.SpawnKitchenObject(currBurningRecipeSO.output, this);

                    // advance to the next state, reset next state timer.
                    state = State.Burned;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });

                    // give 0 so the progress bar ui hides itself
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = 0f });
                }
                break;
                
            case State.Burned: 
                
                break;
        }
    }



    public override void Interact(Player_TopDown player)
    {
        if (!HasKitchenObject())
        {
            // there is no KitchenObject on the counter
            if (player.HasKitchenObject())
            {
                // player is carrying something

                // check if player is holding something that can be fried
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // then move the KitchenObject from player's hand to this Counter.
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    // record the currFryingRecipeSO
                    currFryingRecipeSO = GetFryingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

                    // modify the state to start frying, and reset timer.
                    state = State.Frying;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });
                    
                    fryingTimer = 0f;
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = fryingTimer / currFryingRecipeSO.fryingTimerMax });
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
                GetKitchenObject().SetKitchenObjectParent(player);

                // set the stove state back to idle
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { _state = state });
                // give 0 so the progress bar ui hides itself
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs { progressNormalized = 0f });
            }
        }
    }



    private FryingRecipeSO GetFryingRecipeSOForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }


    private BurningRecipeSO GetBurningRecipeSOForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }



    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOForInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }
}
