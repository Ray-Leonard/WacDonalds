using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation_TopDown : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    private Animator animator;
    private Player_TopDown player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player_TopDown>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}
