using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleOrderUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI orderNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetOrderRecipeSO(OrderRecipeSO orderRecipeSO)
    {
        orderNameText.text = orderRecipeSO.recipeName;

        // cleanup icons
        foreach(Transform child in iconContainer)
        {
            if(child == iconTemplate) { continue; }
            Destroy(child.gameObject);
        }

        // Generate all the icons for this order
        foreach(KitchenObjectSO kitchenObjectSO in orderRecipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);

            // set image
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;
        }
    }
}
