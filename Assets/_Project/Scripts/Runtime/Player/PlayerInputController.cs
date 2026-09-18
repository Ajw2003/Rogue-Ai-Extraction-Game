using Code.Scripts.EventSystems;
using StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerStateMachine _stateMachine;
        private PlayerInputs _input;
        private Vector2 _lookDelta;

        private void Awake()
        {
            _input ??= new PlayerInputs();
            _stateMachine = GetComponent<PlayerStateMachine>();
            EventManager.Instance?.Subscribe(this, (PlayerIdleEvent e) => EnableAllInputs());
        }

        private void Start()
        {
            EnableAllInputs();
        }

        private void Update()
        {
            // Lock player rotation while rotating a held item.
            if (ItemManager.Instance != null && ItemManager.Instance.IsRotatingObject)
            {
                _stateMachine.Look(Vector2.zero);
                return;
            }

            _stateMachine.Look(GetLookDelta());
        }

        private void WalkInputs(bool enable)
        {
            if (enable)
            {
                _input.PlayerActions.Move.started += OnMovePerformed;
                _input.PlayerActions.Move.performed += OnMovePerformed;
                _input.PlayerActions.Move.canceled += OnMoveCanceled;
            }
            else
            {
                _input.PlayerActions.Move.started -= OnMovePerformed;
                _input.PlayerActions.Move.performed -= OnMovePerformed;
                _input.PlayerActions.Move.canceled -= OnMoveCanceled;
            }
        }

        // Opening the inventory used to free the cursor from here. Cursor lock and visibility are
        // now owned solely by CursorLockPolicy, which follows GameState; a second writer is what left
        // the cursor stuck between a menu and the world.
        private void OpenInventoryInput(bool enable)
        {
        }

        private void ItemInteractionInputs(bool enable)
        {
            if (enable)
            {
                _input.Inventory.Enable();
                _input.Inventory.Clicked.started += OnItemClickedPerformed;
                _input.Inventory.Clicked.canceled += OnItemClickedPerformed;
            }
            else
            {
                _input.Inventory.Clicked.started -= OnItemClickedPerformed;
                _input.Inventory.Clicked.canceled -= OnItemClickedPerformed;
                _input.Inventory.Disable();
            }
        }

        private void OnItemClickedPerformed(InputAction.CallbackContext context)
        {
            ItemManager.Instance.OnInventoryClicked(context);
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _stateMachine.ChangeState(_stateMachine.WalkState);
            _stateMachine.Move(context.ReadValue<Vector2>());
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _stateMachine.Move(Vector2.zero);
        }

        private void AttackInputs(bool enable)
        {
            if (enable)
            {
                _input.PlayerActions.Attack.performed += OnAttackPerformed;
            }
            else
            {
                _input.PlayerActions.Attack.performed -= OnAttackPerformed;
            }
        }

        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            _stateMachine.Attack();
            EventManager.Instance?.Publish(new PlayerAttackEvent());
        }

        private void JumpInputs(bool enable)
        {
            if (enable)
            {
                _input.PlayerActions.Jump.performed += OnJumpPerformed;
            }
            else
            {
                _input.PlayerActions.Jump.performed -= OnJumpPerformed;
            }
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            _stateMachine.Jump();
        }

        private void DodgeInputs(bool enable)
        {
            if (enable)
            {
                _input.PlayerActions.Dodge.performed += OnDodgePerformed;
            }
            else
            {
                _input.PlayerActions.Dodge.performed -= OnDodgePerformed;
            }
        }

        private void OnDodgePerformed(InputAction.CallbackContext context)
        {
            _stateMachine.Dodge();
        }

        private void LookInputs(bool enable)
        {
            if (enable)
            {
                _input.PlayerActions.Look.performed += OnLookPerformed;
                _input.PlayerActions.Look.canceled += OnLookPerformed;
            }
            else
            {
                _input.PlayerActions.Look.performed -= OnLookPerformed;
                _input.PlayerActions.Look.canceled -= OnLookPerformed;
            }
        }

        private void OnLookPerformed(InputAction.CallbackContext context)
        {
            _lookDelta = context.ReadValue<Vector2>();
        }

        public Vector2 GetLookDelta()
        {
            return _lookDelta;
        }

        private void DisableAllInputs()
        {
            WalkInputs(false);
            DodgeInputs(false);
            JumpInputs(false);
            AttackInputs(false);
            LookInputs(false);
            OpenInventoryInput(false);
            ItemInteractionInputs(false);
        }

        private void EnableAllInputs()
        {
            if (_input == null) _input = new PlayerInputs();
            _input.Enable();

            OpenInventoryInput(true);
            WalkInputs(true);
            DodgeInputs(true);
            JumpInputs(true);
            AttackInputs(true);
            LookInputs(true);
            ItemInteractionInputs(true);
        }
    }
}
