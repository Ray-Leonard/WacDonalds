using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";
    
    [SerializeField] private ContainerCounter countainerCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        countainerCounter.OnPlayerGrabObject += CountainerCounter_OnPlayerGrabObject;
    }

    private void CountainerCounter_OnPlayerGrabObject(object sender, System.EventArgs e)
    {
        // play the animation of container counter lid opening
        animator.SetTrigger(OPEN_CLOSE);
    }
}
