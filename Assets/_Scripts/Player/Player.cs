﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    [SerializeField] private Transform playerHand;
    private KitchenObject kitchenObject;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More Player Instance");
        }
        Instance = this;
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if(selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }
    private void Update()
    {
        HandleMovement();
        HandleInteractons();
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleInteractons()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter BaseCounter))
            {
                //BaseCounter.Interact();
                if (BaseCounter != selectedCounter)
                {
                    SetSelectedCounter(BaseCounter);
                }

            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    private void HandleMovement()
    {
        Vector2 inputVector =  gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float rotateSpeed = 10f;
        if (moveDir != Vector3.zero) transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        float moveDistance = Time.deltaTime * moveSpeed;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canmove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canmove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canmove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canmove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canmove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canmove)
                {
                    moveDir = moveDirZ;
                }
            }
        }
        if (canmove)
        {
            transform.position += moveDir * moveDistance;
        }
        isWalking = moveDir != Vector3.zero;
       
    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return playerHand;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
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
