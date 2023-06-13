using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject selectedVisualGameObject;
    
    private void Start()
    {
        Player_TopDown.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player_TopDown.OnSelectedCounterChangedEventArgs e)
    {
        // show or hide the selected visual for the counter. (show if the player's selected counter is this counter, and vice versa)
        selectedVisualGameObject.SetActive(e.selectedCounter == clearCounter);
    }
}
