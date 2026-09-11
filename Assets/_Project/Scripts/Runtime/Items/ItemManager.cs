using Code.Scripts.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemManager : SingletonBase<ItemManager>
{
    [SerializeField] private LayerMask _itemLayerMask = -1; // Default to all layers, should be set to "Item" layer
    [SerializeField] private float _dragDistance = 5f;
    [SerializeField] private float _scrollSpeed = 25f;
    [SerializeField] private float _minDragDepth = 0.5f;
    [SerializeField] private float _maxDragDepth = 10f;
    [SerializeField] private float _raycastDistance = 100f;

    [SerializeField] private float _rotationSensitivity = 0.5f;
    [SerializeField] private float _throwForce = 15f;

    private Item _hoveredItem;
    private Item _draggedItem;
    private Camera _mainCamera;
    private float _currentDragDepth;

    protected override bool PersistBetweenScenes => false;

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main;
        if (_mainCamera == null)
        {
            _mainCamera = FindFirstObjectByType<Camera>();
        }
    }

    public bool IsRotatingObject => _draggedItem != null && Mouse.current != null && Mouse.current.middleButton.isPressed;

    private void Update()
    {
        if (_mainCamera == null) return;

        HandleHover();

        if (_draggedItem != null)
        {
            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                ThrowDraggedItem();
                return;
            }

            if (Mouse.current != null && Mouse.current.middleButton.isPressed)
            {
                HandleRotation();
            }
            else
            {
                HandleScrollDepth();
                UpdateDraggedItemPosition();
            }
        }
    }

    private void ThrowDraggedItem()
    {
        if (_draggedItem != null)
        {
            Vector3 throwDirection = _mainCamera.transform.forward;
            _draggedItem.Throw(throwDirection, _throwForce);
            _draggedItem = null;
        }
    }

    private void HandleRotation()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        if (delta.sqrMagnitude > 0.01f)
        {
            Vector3 camRight = _mainCamera.transform.right;
            Vector3 camUp = _mainCamera.transform.up;

            Quaternion rotX = Quaternion.AngleAxis(-delta.y * _rotationSensitivity, camRight);
            Quaternion rotY = Quaternion.AngleAxis(-delta.x * _rotationSensitivity, camUp);

            _draggedItem.UpdateRotation(rotX * rotY * _draggedItem.TargetRotation);
        }
    }

    private void HandleScrollDepth()
    {
        if (Mouse.current == null) return;

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            float normalizedScroll = scroll / 120f;
            _currentDragDepth += normalizedScroll * _scrollSpeed;
            _currentDragDepth = Mathf.Clamp(_currentDragDepth, _minDragDepth, _maxDragDepth);
        }
    }

    private void HandleHover()
    {
        if (_draggedItem != null)
        {
            _hoveredItem = null;
            return;
        }

        if (Mouse.current == null) return;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _itemLayerMask))
        {
            if (hit.collider.TryGetComponent(out Item item))
            {
                _hoveredItem = item;
            }
            else
            {
                _hoveredItem = null;
            }
        }
        else
        {
            _hoveredItem = null;
        }
    }

    private void UpdateDraggedItemPosition()
    {
        if (Mouse.current == null) return;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePos);

        Vector3 targetPoint = ray.GetPoint(_currentDragDepth);
        _draggedItem.UpdateTargetPosition(targetPoint);
    }

    public void OnInventoryClicked(InputAction.CallbackContext context)
    {
        if (context.started && _hoveredItem != null)
        {
            StartDragging(_hoveredItem);
        }
        else if (context.canceled)
        {
            StopDragging();
        }
    }

    public void ForceRelease()
    {
        StopDragging();
    }

    private void StartDragging(Item item)
    {
        _draggedItem = item;
        _draggedItem.StartDragging();

        _currentDragDepth = Vector3.Distance(_mainCamera.transform.position, item.transform.position);
        _currentDragDepth = Mathf.Clamp(_currentDragDepth, _minDragDepth, _maxDragDepth);
    }

    private void StopDragging()
    {
        if (_draggedItem != null)
        {
            _draggedItem.StopDragging();
            _draggedItem = null;
        }
    }

    public Item HoveredItem => _hoveredItem;
}
