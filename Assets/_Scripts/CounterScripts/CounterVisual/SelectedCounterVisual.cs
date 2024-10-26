using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] selectedVisualGameObjectArray;
    
    private void Start()
    {
        Player_TopDown.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void OnDestroy()
    {
        Player_TopDown.Instance.OnSelectedCounterChanged -= Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player_TopDown.OnSelectedCounterChangedEventArgs e)
    {
        // show or hide the selected visual for the counter. (show if the player's selected counter is this counter, and vice versa)

        if(e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach(var item in selectedVisualGameObjectArray)
        {
            item.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach(var item in selectedVisualGameObjectArray)
        {
            item.SetActive(false);
        }
    }

}
