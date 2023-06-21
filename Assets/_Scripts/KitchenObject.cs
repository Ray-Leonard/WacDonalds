using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // the kitchen object also keeps a reference to the kitchen object parent, to know where itself is placed. 
    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() 
    {
        return kitchenObjectSO; 
    }

    /// <summary>
    /// 1. Notify the prev Kitchen Object Parent that this kitchen object is no longer there.
    /// 2. Set the current kitchenObjectParent
    /// 3. Notify the new (current) parent that this kitchen object is here.
    /// 4. Move the visual of the kitchen object to the new parent.
    /// </summary>
    /// <param name="kitchenObjectParent"></param>
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        // tell the prev parent that this kitchen object is no longer there.
        if(this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        // set the current kitchenObjectParent
        this.kitchenObjectParent = kitchenObjectParent;


        if(kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent Already has a Kitchen Object!");
        }

        // tell the new (current) parent that this object is here.
        kitchenObjectParent.SetKitchenObject(this);

        // move the visual of the kitchen object to the new parent
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }


    public IKitchenObjectParent GetKitchenObjectParent() 
    {
        return kitchenObjectParent;
    }
}
