using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // not valid ingredient
            return false;
        }
        
        // for now, the design is not allowing duplicated ingredients. But I will extend the system to allow things like triple meat double cheese burger
        if(kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // already contains this type, cannot add
            return false;
        }
        else
        {
            // only add the KitchenObjectSO to the list.
            kitchenObjectSOList.Add(kitchenObjectSO);

            return true;
        }
    }
}