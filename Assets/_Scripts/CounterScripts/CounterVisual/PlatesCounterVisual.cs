using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    
    // for spawning the plate object visual
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    // for plate stacking visual
    private float plateOffsetY = 0.1f;
    private Stack<GameObject> plateVisualGameObjectStack;

    private void Awake()
    {
        plateVisualGameObjectStack = new Stack<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        // remove the top one from the stack
        GameObject plateToRemove = plateVisualGameObjectStack.Pop();

        // destroy the game object
        Destroy(plateToRemove);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        // offset plate visual Y value
        plateVisualTransform.localPosition = new Vector3(0, plateVisualGameObjectStack.Count * plateOffsetY, 0);

        // add plate to the list
        plateVisualGameObjectStack.Push(plateVisualTransform.gameObject);
    }
}
