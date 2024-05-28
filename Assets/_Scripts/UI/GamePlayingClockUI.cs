using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private void Awake()
    {
        timerImage.fillAmount = 1;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
        }
    }
}
