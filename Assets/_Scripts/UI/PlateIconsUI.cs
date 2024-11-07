using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;

    // template to spawn more icons
    [SerializeField] private Transform iconTemplate;


    private void Awake()
    {
        // disable template at awake
        iconTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void OnDestroy()
    {
        if(plateKitchenObject != null)
        {
            plateKitchenObject.OnIngredientAdded -= PlateKitchenObject_OnIngredientAdded;
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }


    /// <summary>
    /// update the icons visual
    /// </summary>
    private void UpdateVisual()
    {
        // clean up the previously spawned icons (but dont delete the template)
        foreach(Transform child in transform)
        {
            if(child == iconTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }


        // loop through the kitchenObjectSO list in the plateKitchenObject
        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            // instantiate a new icon template
            PlateIconSingleUI plateIconSingleUI = Instantiate(iconTemplate, transform).GetComponent<PlateIconSingleUI>();

            // enable the game object (it is disabled because it is copied from the disabled iconTemplate)
            plateIconSingleUI.gameObject.SetActive(true);

            // update the icon sprite by setting kitchenObjectSO
            plateIconSingleUI.SetKitchenObjectSO(kitchenObjectSO);
        }
    }
}
