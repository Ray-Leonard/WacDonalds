using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    
    // interface is not serializable, so we use hasProgressGameObject and then get the component at start. 
    private IHasProgress progressItem;


    private void Start()
    {
        progressItem = hasProgressGameObject.GetComponent<IHasProgress>();

        if(progressItem == null ) 
        {
            Debug.LogError(hasProgressGameObject + " does not have a component that implements IHasProgress!");
        }

        progressItem.OnProgressChanged += ProgressItem_OnProgressChanged;
        barImage.fillAmount = 0;
        Hide();
    }

    private void OnDestroy()
    {
        progressItem.OnProgressChanged -= ProgressItem_OnProgressChanged;
    }


    private void ProgressItem_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if(e.progressNormalized == 0 || e.progressNormalized == 1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }


    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
