using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TouchManager : MonoBehaviour
{
    private Camera mainCamera;

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private PointerEventData pointerEventData;

    private Vector2 initialTouchPosition;
    private float touchStartTime;
    private float timeThreshold = 0.2f;

    [SerializeField]
    private float raycastDistance = 100f; // Maximum distance for raycasting to detect interactable objects
    [SerializeField]
    private LayerMask mask; // Layer mask for raycasting

    // Initialization methods ------------------------------------------------------------
    private bool SetUpCameraAndInput() {
        bool isValid = true;

        mainCamera = Camera.main;
        if (mainCamera == null) {
            HandleError($"[TouchManager] Main camera not found! " +
                $"Ensure there is a camera in the scene tagged as 'MainCamera'.");
            isValid = false;
        }

        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null) {
            HandleError($"[TouchManager] PlayerInput component not found on {gameObject.name}.");
            isValid = false;
        }

        try {
            touchPressAction = playerInput.actions["TouchPress"];
            touchPositionAction = playerInput.actions["TouchPosition"];
        } catch (KeyNotFoundException) {
            HandleError($"[TouchManager] Required input actions 'TouchPress' and/or 'TouchPosition' not found in PlayerInput actions.");
            isValid = false;
        }
        
        if (EventSystem.current == null) {
            HandleError($"[TouchManager] EventSystem not found in the scene. UI interactions will not be detected.");
            isValid = false;
        }
        else
            pointerEventData = new PointerEventData(EventSystem.current);

        return isValid;
    }

    private void HandleError(string message) {
        Debug.LogError(message);
    }

    // Lifecycle methods ------------------------------------------------------------
    private void Awake()
    {   
        if (!SetUpCameraAndInput()) {
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        touchPressAction.performed += OnTouchStarted;
        touchPressAction.canceled += OnTouchEnded;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= OnTouchStarted;
        touchPressAction.canceled -= OnTouchEnded;
    }

    // Touch event handlers ------------------------------------------------------------
    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        initialTouchPosition = touchPositionAction.ReadValue<Vector2>();
        touchStartTime = Time.time;
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        float touchDuration = Time.time - touchStartTime;
        if (touchDuration < timeThreshold)
            HandleTouch(initialTouchPosition);
    }

    // Core interaction logic ------------------------------------------------------------
    private void HandleTouch(Vector2 touchPosition)
    {
        if (IsPointerOverUI(touchPosition)) return;

        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(touchPosition);

        if (Physics.Raycast(ray, out hit, raycastDistance, mask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                interactable.Interact();
                return;
            }
        }
    }

    private bool IsPointerOverUI(Vector2 touchPosition)
    {
        pointerEventData ??= new PointerEventData(EventSystem.current);

        pointerEventData.position = touchPosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        return raycastResults.Count > 0;
    }
}
