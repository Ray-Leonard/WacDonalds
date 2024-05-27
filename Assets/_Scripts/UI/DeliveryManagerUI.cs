using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform orderTemplate;

    private void Awake()
    {
        orderTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderSpawned += DeliveryManager_OnOrderSpawned;
        DeliveryManager.Instance.OnOrderCompleted += DeliveryManager_OnOrderCompleted;

        // Make sure the default ones in scene are cleared by updating visual ocne on start.
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        // destroy previous 
        foreach (Transform child in container)
        {
            if (child == orderTemplate)
            {
                continue;
            }

            Destroy(child.gameObject);
        }

        // create new ones based on current state
        foreach(OrderRecipeSO orderSO in DeliveryManager.Instance.GetWaitingOrderSOList())
        {
            Transform orderTransform = Instantiate(orderTemplate, container);
            orderTransform.gameObject.SetActive(true);

            // inject SO data
            orderTransform.GetComponent<DeliveryManagerSingleOrderUI>().SetOrderRecipeSO(orderSO);
        }
    }
}
