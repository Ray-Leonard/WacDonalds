using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private MenuSO menuSO;

    private List<OrderRecipeSO> waitingRecipeSOList;
}
