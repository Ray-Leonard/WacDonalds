using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This MonoBehaviour will let the SceneLoader static class know when the first update happens.
/// </summary>
public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    // Update is called once per frame
    void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;

            // the first frame is executed, callback.
            SceneLoader.LoaderCallback();
        }
    }
}
