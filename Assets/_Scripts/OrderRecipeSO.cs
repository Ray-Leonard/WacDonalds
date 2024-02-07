using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/OrderRecipeSO")]
public class OrderRecipeSO : ScriptableObject
{
    // a list of ingredients
    public List<KitchenObjectSO> kitchenObjectSOList;

    public string recipeName;
}
