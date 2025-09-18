
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float speed = 5f;

    private Rigidbody rb;
    public Vector2 moveInput;

    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            // Disable the script and the PlayerInput component if we are not the owner
            enabled = false;
            if (playerInput != null)
            {
                playerInput.enabled = false;
            }
            return;
        }
    }

    private void FixedUpdate()
    {
         if (!IsOwner) return;

        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        rb.linearVelocity = moveDirection * speed;
        Debug.Log(rb.linearVelocity);
    }

    public void Move(Vector2 movement)
    {
        moveInput = movement;
        Debug.Log(moveInput);
    }
}
