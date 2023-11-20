using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    [SerializeField] private GameObject stoveOnEffect;
    [SerializeField] private GameObject particleEffect;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showVisual = e._state == StoveCounter.State.Frying || e._state == StoveCounter.State.Cooked;

        stoveOnEffect.SetActive(showVisual);
        particleEffect.SetActive(showVisual);
    }
}
