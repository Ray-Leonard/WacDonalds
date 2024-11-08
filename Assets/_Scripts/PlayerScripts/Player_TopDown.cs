using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_TopDown : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler OnPickedSomething;

    public static Player_TopDown Instance { get; private set; }
    
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
    private BaseCounter selectedCounter;

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    // event argument class that extens EventArgs
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;


    private bool isWalking;


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        // listen to gameInput's OnInteractAction event
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void OnDestroy()
    {
        if(GameInput.Instance != null)
        {
            GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
            GameInput.Instance.OnInteractAlternateAction -= GameInput_OnInteractAlternateAction;
        }
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
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetTopDownMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        // record the moveDir into lastInteractDir so that lastInteractDir is also not zero when player is not moving.
        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }


        bool canInteract = false;
        RaycastHit raycastHit;
        BaseCounter hitCounter = null;
        // determine if there's a counter *in front* (same as moving dir) of the player that can interact with. 
        if(Physics.Raycast(transform.position, lastInteractDir, out raycastHit, interactDistance, countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out hitCounter))
            {
                canInteract = true;
            }
        }



        if (!canInteract)
        {
            // attempt left
            // rotate the lastInteractDir -90 degrees to check if there's something to interact with on the left
            Vector3 interactDirLeft = Quaternion.Euler(0, -90, 0) * lastInteractDir;
            if (Physics.Raycast(transform.position, interactDirLeft, out raycastHit, interactDistance, countersLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out hitCounter))
                {
                    canInteract = true;
                }
            }

            if (canInteract)
            {
                // record the interact dir
                lastInteractDir = interactDirLeft;
            }
            else
            {
                // attempt right
                // rotate the lastInteractDir 90 degrees to check if there's something to interact with on the left
                Vector3 interactDirRight = Quaternion.Euler(0, 90, 0) * lastInteractDir;
                if (Physics.Raycast(transform.position, interactDirRight, out raycastHit, interactDistance, countersLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out hitCounter))
                    {
                        canInteract = true;
                    }
                }

                if (canInteract)
                {
                    // record direction
                    lastInteractDir = interactDirRight;
                }
                else
                {
                    // attempt back
                    // rotate the lastInteractDir 180 degrees to check if there's something to interact with on the left
                    Vector3 interactDirback = Quaternion.Euler(0, 180, 0) * lastInteractDir;
                    if (Physics.Raycast(transform.position, interactDirback, out raycastHit, interactDistance, countersLayerMask))
                    {
                        if (raycastHit.transform.TryGetComponent(out hitCounter))
                        {
                            canInteract = true;
                        }
                    }

                    if (canInteract)
                    {
                        // record direction
                        lastInteractDir = interactDirback;
                    }
                    else
                    {
                        // do nothing
                    }
                }

            }
        }



        // finally assign the counters according to canInteract.
        if (canInteract)
        {
            // record the selected counter when a different one has been selected.
            if (hitCounter != selectedCounter)
            {
                SetSelectedCounter(hitCounter);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }


    private void HandleMovement()
    {
        /// Get player input and calculate move direction
        Vector2 inputVector = GameInput.Instance.GetTopDownMovementVectorNormalized();
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

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        // Fire selected counter changed event
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }


    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        // check player pick up something
        if(kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
