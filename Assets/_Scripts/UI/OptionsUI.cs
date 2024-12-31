using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button closeButton;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogError("There is more than one instance of OptionsUI");
        }

        sfxSlider.onValueChanged.AddListener((float val) => SoundManager.Instance.SetVolume(val));
        musicSlider.onValueChanged.AddListener((float val) => MusicManager.Instance.SetVolume(val));
        closeButton.onClick.AddListener(() => Hide());
    }

    private void Start()
    {
        // update slider default value
        sfxSlider.SetValueWithoutNotify(SoundManager.Instance.GetVolume());
        musicSlider.SetValueWithoutNotify(MusicManager.Instance.GetVolume());

        // hide the options UI by default
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
