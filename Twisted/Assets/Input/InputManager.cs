using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static event Action<bool> OnMouseLeftClick;
    public static event Action OnLoad;
    public static event Action OnSave;
    public static event Action OnTab;
    public static event Action<int> OnNumPads;
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

    private void OpenInventory(InputAction.CallbackContext context)
    {
        OnTab?.Invoke();
    }

    private void LoadButton(InputAction.CallbackContext context)
    {
        OnLoad?.Invoke();
    }

    private void SaveButton(InputAction.CallbackContext context)
    {
        OnSave?.Invoke();
    }
    private void NumPadsPressed(InputAction.CallbackContext context)
    {
        string cxt = context.control.name;

        if (int.TryParse(cxt, out int num))
        {
            OnNumPads?.Invoke(num);
        }
    }

    private void OnEnable()
    {
        inputBase.Enable();
        inputBase.Player.MouseLeftClick.started += MouseLeftClick;
        inputBase.Player.MouseLeftClick.canceled += MouseLeftClick;
        inputBase.Player.Save.started += SaveButton;
        inputBase.Player.Load.started += LoadButton;
        inputBase.Player.Tab.started += OpenInventory;
        inputBase.Player.NumPads.performed += NumPadsPressed;
    }


    private void OnDisable()
    {
        inputBase.Disable();
        inputBase.Player.MouseLeftClick.started -= MouseLeftClick;
        inputBase.Player.MouseLeftClick.canceled -= MouseLeftClick;
        inputBase.Player.Save.started -= SaveButton;
        inputBase.Player.Load.started -= LoadButton;
        inputBase.Player.Tab.started -= OpenInventory;
        inputBase.Player.NumPads.performed -= NumPadsPressed;
    }

}
