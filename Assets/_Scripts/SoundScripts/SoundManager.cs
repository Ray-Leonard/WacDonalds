using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClipRefsSO audioDatabase;

    private void Start()
    {
        DeliveryManager.Instance.OnOrderSuccess += DeliveryManager_OnOrderSuccess;
        DeliveryManager.Instance.OnOrderFailed += DeliveryManager_OnOrderFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player_TopDown.Instance.OnPickedSomething += PlayerTopDown_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
        PlayerSound_TopDown.OnPlayerTopDownFootstep += PlayerSound_TopDown_OnPlayerTopDownFootstep;

    }

    private void PlayerSound_TopDown_OnPlayerTopDownFootstep(object sender, System.EventArgs e)
    {
        PlayerSound_TopDown playerSound = sender as PlayerSound_TopDown;
        PlaySound(audioDatabase.footstep, playerSound.transform.position);
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioDatabase.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioDatabase.objectDrop, baseCounter.transform.position);
    }

    private void PlayerTopDown_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(audioDatabase.objectPickup, Player_TopDown.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        // we know for sure the sender is of type CuttingCounter, so it's safe to cast. 
        // and we need this CuttingCounter because we want to know the position of the sound.
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioDatabase.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnOrderFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioDatabase.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_OnOrderSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioDatabase.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }


    private void PlaySound(AudioClip[] audioClipArr, Vector3 position, float volume = 1)
    {
        AudioSource.PlayClipAtPoint(audioClipArr[Random.Range(0, audioClipArr.Length)], position, volume);
    }


    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}