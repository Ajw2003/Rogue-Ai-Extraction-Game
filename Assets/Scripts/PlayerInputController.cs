using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInputs  _playerInputs;
    private PlayerController _playerController;


    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if (_playerInputs == null)
            _playerInputs = new PlayerInputs();
        
        _playerInputs.Enable();
        _playerInputs.Movement.Move.performed += OnMove;

    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        // Debug.Log(context.ReadValue<Vector2>());
        _playerController.Move(context.ReadValue<Vector2>());
    }
}
