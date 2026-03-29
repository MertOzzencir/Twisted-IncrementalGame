using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static event Action<bool> OnMouseLeftClick;
    private InputActions inputBase;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        inputBase = new InputActions();
    }
    void Update()
    {
        //Debug.Log(MousePosition());
    }
    public Vector2 MovementVector()
    {
        Vector2 movement = inputBase.Player.Move.ReadValue<Vector2>();
        return movement;
    }
    public Vector2 MousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    private void MouseLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
            OnMouseLeftClick?.Invoke(true);
        if (context.canceled)
            OnMouseLeftClick?.Invoke(false);
    }

    private void OnEnable()
    {
        inputBase.Enable();
        inputBase.Player.MouseLeftClick.started += MouseLeftClick;
        inputBase.Player.MouseLeftClick.canceled += MouseLeftClick;


    }
    private void OnDisable()
    {
        inputBase.Disable();
        inputBase.Player.MouseLeftClick.started -= MouseLeftClick;
        inputBase.Player.MouseLeftClick.canceled -= MouseLeftClick;
    }

}
