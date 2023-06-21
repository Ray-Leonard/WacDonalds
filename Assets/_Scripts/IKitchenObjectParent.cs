
using UnityEngine;

/// <summary>
/// IKitchenObjectParent defines that the class can hold a kitchen object.
/// </summary>
public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform();

    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();

    public bool HasKitchenObject();
}
