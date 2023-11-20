using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] private Transform counterTopPoint;

    // keep a reference to the current kitchen object so the counter knows if there's something on top of it now. 
    private KitchenObject kitchenObject;

    public virtual void Interact(Player_TopDown player)
    {
        Debug.LogError("BaseCounter.Interact() should never be triggered!");
    }

    public virtual void InteractAlternate(Player_TopDown player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate() should never be triggered!");
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
