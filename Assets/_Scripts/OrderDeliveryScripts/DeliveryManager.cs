using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnOrderSpawned;
    public event EventHandler OnOrderCompleted;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private MenuSO menuSO;             // contains all the recipe SO.

    private List<OrderRecipeSO> waitingOrderSOList;    // for all the recipes that's waiting to be completed.

    private float spawnOrderTimer;
    private float spawnOrderTimerMax = 4f;
    private int waitingOrdersMax = 4;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this; 

        waitingOrderSOList= new List<OrderRecipeSO>();
    }

    private void Update()
    {
        spawnOrderTimer -= Time.deltaTime;
        if(spawnOrderTimer <= 0f)
        {
            // reset timer
            spawnOrderTimer = spawnOrderTimerMax;

            if(waitingOrderSOList.Count < waitingOrdersMax)
            {
                // get a new recipe from menu and add to waitingOrderList
                OrderRecipeSO waitingRecipeSO = menuSO.orderRecipeSOList[UnityEngine.Random.Range(0, menuSO.orderRecipeSOList.Count)];
                waitingOrderSOList.Add(waitingRecipeSO);

                OnOrderSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Called when a order is delivered to validate if the order is correct.
    /// Simply cycle through the waitingOrderSOList and see if the kitchenObjectSO on the plate is contained in any of the waiting orders
    /// </summary>
    /// <param name="plateKitchenObject">The order will be delivered on a plate</param>
    public void DeliverOrder(PlateKitchenObject plateKitchenObject)
    {
        List<KitchenObjectSO> plateKitchenObjectSOList = plateKitchenObject.GetKitchenObjectSOList();

        for(int i = 0; i < waitingOrderSOList.Count; i++)
        {
            OrderRecipeSO waitingOrderRecipeSO = waitingOrderSOList[i];

            // first check if the order and plate has the same number of ingredients. If checked, then proceed to check the ingredients. 
            if(waitingOrderRecipeSO.kitchenObjectSOList.Count == plateKitchenObjectSOList.Count)
            {
                bool isPlateContentMatchesRecipe = true;
                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingOrderRecipeSO.kitchenObjectSOList)
                {
                    if (!plateKitchenObjectSOList.Contains(recipeKitchenObjectSO))
                    {
                        // plate does not contain the order's ingredient, ingredient not found, therefore plate content does not match this recipe.
                        isPlateContentMatchesRecipe = false;
                        break;
                    }
                }


                // now if the previous foreach loop does not set isPlateContentMatchesRecipe to false, 
                // that means the plate matches with the order
                if(isPlateContentMatchesRecipe)
                {
                    // remove the order from the waiting list.
                    waitingOrderSOList.RemoveAt(i);
                    // does not need to continue the search

                    OnOrderCompleted?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // otherwise, if none of the search finds the correct order, it is a wrong delivery.
        Debug.Log("Player did not deliver a correct recipe");
    }

    public List<OrderRecipeSO> GetWaitingOrderSOList()
    {
        return waitingOrderSOList;  
    }
}
