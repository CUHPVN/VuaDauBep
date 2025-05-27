using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput instance {  get; private set; }
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    private InputSystem_Actions playerInputActions;



    private void Awake()
    {
        instance = this;
        playerInputActions = new InputSystem_Actions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnInteractAction != null)
        {
            OnInteractAction(this, EventArgs.Empty); 
        }
    }
    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (OnInteractAlternateAction != null)
        {
            OnInteractAlternateAction(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }
}
