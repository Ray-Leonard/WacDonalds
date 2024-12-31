using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        Hide();
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
            GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
        }
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
        // also hide the OptionsUI if it's by any chance open.
        OptionsUI.Instance.Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
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
