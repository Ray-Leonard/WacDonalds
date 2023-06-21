using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // keep a reference to the current kitchen object so the counter knows if there's something on top of it now. 
    private KitchenObject kitchenObject;

    public void Interact(Player_TopDown player)
    {
        // when the counter is empty
        if(kitchenObject == null)
        {
            // spawn the kitchen object.
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);

        }
        // when there is already a kitchen object on the counter.
        else
        {
            // give the object to the player
            kitchenObject.SetKitchenObjectParent(player);
        }

    }

    public Transform GetKitchenObjectFollowTransform() 
    {
        return counterTopPoint;
    }


    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
