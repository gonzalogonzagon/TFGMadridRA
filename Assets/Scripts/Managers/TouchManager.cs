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

    [SerializeField]
    private float raycastDistance = 100f; // Maximum distance for raycasting to detect interactable objects
    [SerializeField]
    private LayerMask mask; // Layer mask for raycasting

    [SerializeField]
    private float dragThreshold = 0.2f; // Threshold to distinguish between tap and drag

    [SerializeField] private GameObject infoPanel; // Hides info panel when no interactable is touched

    // Lifecycle methods ------------------------------------------------------------
    private void Awake()
    {   
        if (EventSystem.current != null)
            pointerEventData = new PointerEventData(EventSystem.current);
        
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null) ////
        {
            Debug.LogError("PlayerInput component not found on " + gameObject.name);
        } ////

        // Initialize input actions
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];

        mainCamera = Camera.main;
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
        if (touchDuration < dragThreshold)
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
        if (infoPanel != null && infoPanel.activeSelf) infoPanel.SetActive(false);
    }

    private bool IsPointerOverUI(Vector2 touchPosition)
    {
        if (EventSystem.current == null) {
            Debug.LogWarning("EventSystem not found in the scene.");
            return false;
        }

        pointerEventData ??= new PointerEventData(EventSystem.current);

        pointerEventData.position = touchPosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        return raycastResults.Count > 0;
    }
}
