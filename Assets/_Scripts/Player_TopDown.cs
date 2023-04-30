using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_TopDown : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;

    private void Update()
    {

        Vector2 inputVector = gameInput.GetTopDownMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        
        isWalking = moveDir != Vector3.zero;

        // apply position change
        transform.position += moveDir * Time.deltaTime * movementSpeed;

        if(isWalking )
        {
            // apply rotation change (by changing player's forward vector)
            // Slerp is usually used for lerping rotations, while Lerp is good for positions
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }

    }


    /// <summary>
    /// Returns true if playing is walking, false otherwise
    /// Meant to be used by player's Animation controller to query the state of the player. 
    /// </summary>
    /// <returns></returns>
    public bool IsWalking()
    {
        return isWalking;
    }
}
