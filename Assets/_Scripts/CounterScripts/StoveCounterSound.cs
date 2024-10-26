using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void OnDestroy()
    {
        stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if(e._state == StoveCounter.State.Frying || e._state == StoveCounter.State.Cooked)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
