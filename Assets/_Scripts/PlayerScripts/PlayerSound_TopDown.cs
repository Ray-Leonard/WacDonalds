using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSound_TopDown : MonoBehaviour
{
    public static event EventHandler OnPlayerTopDownFootstep;

    public static void ResetStaticData()
    {
        OnPlayerTopDownFootstep = null;
    }

    private Player_TopDown player;
    private float footstepTimer;
    private float footstepTimerMax = 0.1f;

    private void Awake()
    {
        player = GetComponent<Player_TopDown>();
    }

    private void Update()
    {
        if (player.IsWalking())
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer < 0f)
            {
                footstepTimer = footstepTimerMax;

                OnPlayerTopDownFootstep?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
