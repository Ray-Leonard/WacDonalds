using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_TopDown : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Collision")]
    [SerializeField] private float playerRadius;
    [SerializeField] private float playerHeight;

    [Header("Interaction")]
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask countersLayerMask;
    private Vector3 lastInteractDir;

    private bool isWalking;

    private void Start()
    {
        // listen to gameInput's OnInteractAction event
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }



    private void Update()
    {
        HandleMovement();
        HandleInteractions();
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

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetTopDownMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        // record the moveDir into lastInteractDir so that lastInteractDir is also not zero when player is not moving.
        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        // use raycast to see if there's anything in front of the player that they can interact with.
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            // we're hitting a ClearCounter
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //clearCounter.Interact();
            }
        }
    }


    private void HandleMovement()
    {
        /// Get player input and calculate move direction
        Vector2 inputVector = gameInput.GetTopDownMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        isWalking = moveDir != Vector3.zero;
        if (isWalking)
        {
            /// apply rotation change (by changing player's forward vector)
            // Slerp is usually used for lerping rotations, while Lerp is good for positions
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);

            /// use Physics.CapsuleCast to detect player collision. 
            /* REFLECTION:
             * Why we use Physics function instead of adding collider/rigidbody to the player and let Unity's physics system take care of collision detection? 
             * 
             * 1. The game design entails that the player will always be moving on the flat floor, with no y-axis movement.
             *    In this case, using rigidbody, we have to not allow gravity.
             *     
             * 2. We're already directly moving player's transform.position and setting player's transform.forward to rotate the player in the code, 
             *    if we use rigidbody, we'll have to lock rigidbody's rotation, and refactor the code to use Rigidbody.MovePosition() to use the benefits of Unity's physics system.
             *     
             * 3. Given the game design's simple use case, it's actually more efficient to use our own "trimmed" version of collision detection, because physics system has 
             *    more complications which we won't use. 
             * 
             * NOTE FROM CODEMONKEY:
             * As usual in many things related to Game Development there are always a multitude of options to achieve the same result. 
             * You could definitely use a Rigidbody and a Collider to achieve collision detection, that would indeed work and I've used that method in many games.
             * So for the most part the answer is personal preference.
             * However one of the reasons why I made this game is because the goal is for the next course to cover multiplayer, 
             * and when working with multiplayer it does help to avoid using Physics as much as possible.
             * So in Multiplayer a non-Rigidbody controller is probably easier, but in Singleplayer both work just fine, you can use whatever you prefer.
             */

            float moveDistance = Time.deltaTime * movementSpeed;
            bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

            // When Cannot move, split the detection into X and Z separately so we can handle the case when player's walking diagonally into a wall. 
            if (!canMove)
            {
                // Can't move towards moveDir
                // Attempt only X movement
                Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
                if (canMove)
                {
                    // can move on X, make the moveDir to be moveDirX
                    moveDir = moveDirX;
                }
                else
                {
                    // Cannot move on X
                    // Attempt only Z movement
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                    canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                    else
                    {
                        // cannot move in any direction
                    }
                }
            }

            if (canMove)
            {
                // apply position change
                transform.position += moveDir * moveDistance;
            }
        }
    }
}
