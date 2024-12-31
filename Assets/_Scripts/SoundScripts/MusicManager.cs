using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;

    private float volume = 0.2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("There's more than one instance of MusicManager");
        }
        audioSource = GetComponent<AudioSource>();

        // load the volume from player prefs and set the volume to audio source
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        audioSource.volume = volume;
    }


    public void SetVolume(float val)
    {
        volume = val;
        // save the volume to player prefs
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);

        audioSource.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }
}
