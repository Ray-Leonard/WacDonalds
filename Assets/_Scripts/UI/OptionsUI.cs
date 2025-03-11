using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button closeButton;

    [Header("Key Bindings")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject waitingForInputPanel;

    private Dictionary<Button, TextMeshProUGUI> buttonTextDict = new();

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

        // register volume slider listeners
        sfxSlider.onValueChanged.AddListener((float val) => SoundManager.Instance.SetVolume(val));
        musicSlider.onValueChanged.AddListener((float val) => MusicManager.Instance.SetVolume(val));
        closeButton.onClick.AddListener(() => Hide());

        // register key rebind button listeners
        moveUpButton.onClick.AddListener(() => RebindKeyBinding(GameInput.Binding.MoveUp));
        moveDownButton.onClick.AddListener(() => RebindKeyBinding(GameInput.Binding.MoveDown));
        moveLeftButton.onClick.AddListener(() => RebindKeyBinding(GameInput.Binding.MoveLeft));
        moveRightButton.onClick.AddListener(() => RebindKeyBinding(GameInput.Binding.MoveRight));
        interactButton.onClick.AddListener(() => RebindKeyBinding(GameInput.Binding.Interact));
        interactAltButton.onClick.AddListener(() => RebindKeyBinding(GameInput.Binding.InteractAlt));
        pauseButton.onClick.AddListener(() => RebindKeyBinding(GameInput.Binding.Pause));
    }

    private void Start()
    {
        // update slider default value
        sfxSlider.SetValueWithoutNotify(SoundManager.Instance.GetVolume());
        musicSlider.SetValueWithoutNotify(MusicManager.Instance.GetVolume());

        // init button text
        buttonTextDict.Add(moveUpButton, moveUpButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        buttonTextDict.Add(moveDownButton, moveDownButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        buttonTextDict.Add(moveLeftButton, moveLeftButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        buttonTextDict.Add(moveRightButton, moveRightButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        buttonTextDict.Add(interactButton, interactButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        buttonTextDict.Add(interactAltButton, interactAltButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        buttonTextDict.Add(pauseButton, pauseButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        UpdateButtonVisuals();

        // hide the options UI by default
        Hide();
    }

    private void UpdateButtonVisuals()
    {
        SetButtonVisual(moveUpButton, GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp));
        SetButtonVisual(moveDownButton, GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown));
        SetButtonVisual(moveLeftButton, GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft));
        SetButtonVisual(moveRightButton, GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight));
        SetButtonVisual(interactButton, GameInput.Instance.GetBindingText(GameInput.Binding.Interact));
        SetButtonVisual(interactAltButton, GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt));
        SetButtonVisual(pauseButton, GameInput.Instance.GetBindingText(GameInput.Binding.Pause));
    }

    private void SetButtonVisual(Button button, string label)
    {
        if(buttonTextDict.TryGetValue(button, out TextMeshProUGUI buttonText))
        {
            buttonText.text = label;

            // make the button longer if laber length is more than 1
            if (label.Length > 1)
            {
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 50);
            }
            else
            {
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            }
        }
    }

    private void RebindKeyBinding(GameInput.Binding binding)
    {
        ShowWaitForInputPanel();

        GameInput.Instance.RebindBinding(binding, () => { 
            HideWaitForInputPanel();
            UpdateButtonVisuals();
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowWaitForInputPanel()
    {
        waitingForInputPanel.SetActive(true);
    }

    private void HideWaitForInputPanel()
    {
        waitingForInputPanel.SetActive(false);
    }
}
