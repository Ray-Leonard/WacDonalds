using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A MenuSO contains all the OrderRecipeSO.
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/MenuSO")]
public class MenuSO : ScriptableObject
{
    public List<OrderRecipeSO> orderRecipeSOList;
}
