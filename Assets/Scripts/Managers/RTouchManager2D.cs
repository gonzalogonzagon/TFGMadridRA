using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.EventSystems;

public class RTouchManager2D : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction pinchAction;

    private Camera mainCamera;
    private Vector3 targetCameraPosition;

    private PointerEventData pointerEventData;

    private Vector2 initialTouchPosition;
    private float touchStartTime;
    private bool isDragging;
    private bool isZooming;

    private float minX, maxX, minY, maxY;
    private float targetOrthoSize;

    [SerializeField]
    private float distance = 50f; // Maximum distance for raycasting to detect interactable objects
    [SerializeField]
    private LayerMask mask; // Layer mask for raycasting
    [SerializeField]
    private Transform targetContent; // The content to pan within boundaries 

    [SerializeField]
    private float dragThreshold = 0.2f; // Threshold to distinguish between tap and drag
    [SerializeField]
    private float cameraSmoothSpeed = 10f; // Speed of camera smoothing

    [SerializeField]
    private float cameraPositionSpeed = 6f; // Adjust panning speed as needed
    [SerializeField]
    private float cameraZoomSpeed = 0.02f; // Adjust camera zoom speed as needed
    [SerializeField]
    private float minOrthoSize = 1f; // Minimum field of view for zooming
    [SerializeField]
    private float maxOrthoSize = 5f; // Maximum field of view for zooming

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

        if (!EnhancedTouchSupport.enabled)
            EnhancedTouchSupport.Enable();

        // Initialize input actions
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
        pinchAction = playerInput.actions["TouchPinch"];

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("No camera tagged as MainCamera found in the scene.");
            enabled = false;
            return;
        }

        targetOrthoSize = mainCamera.orthographicSize;
        targetCameraPosition = mainCamera.transform.position;

        UpdateCameraBounds();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        touchPressAction.performed += OnTouchStarted;
        touchPressAction.canceled += OnTouchEnded;
        touchPositionAction.performed += OnTouchMoved;
        pinchAction.performed += OnPinchStart;
        pinchAction.canceled += OnPinchEnd;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= OnTouchStarted;
        touchPressAction.canceled -= OnTouchEnded;
        touchPositionAction.performed -= OnTouchMoved;
        pinchAction.performed -= OnPinchStart;
        pinchAction.canceled -= OnPinchEnd;
    }

    private void Update()
    {
        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            targetOrthoSize,
            Time.deltaTime * cameraSmoothSpeed
        );

        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            targetCameraPosition,
            Time.deltaTime * cameraSmoothSpeed
        );
    }

    // Touch event handlers ------------------------------------------------------------
    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        initialTouchPosition = touchPositionAction.ReadValue<Vector2>();
        isDragging = true;
        touchStartTime = Time.time;
    }

    private void OnTouchMoved(InputAction.CallbackContext context)
    {
        if (!isDragging || isZooming) return;

        Vector2 currentTouchPosition = touchPositionAction.ReadValue<Vector2>();
        Vector2 delta = currentTouchPosition - initialTouchPosition;

        // Scale factor based on orthographic size and screen resolution
        float orthoFactor = targetOrthoSize * 2f / Screen.height;
        // Delta movement in world space units
        Vector3 worldDelta = new Vector3(
            -delta.x * orthoFactor * cameraPositionSpeed, 
            -delta.y * orthoFactor * cameraPositionSpeed, 0);

        Vector3 newPosition = mainCamera.transform.position + worldDelta;

        // Limit panning within boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        targetCameraPosition = newPosition;

        // Update initial touch position for the next delta calculation
        initialTouchPosition = currentTouchPosition;
    }

    private void OnTouchEnded(InputAction.CallbackContext context)
    {
        isDragging = false;
        if (isZooming) isZooming = false;

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

        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract())
            {
                interactable.Interact();
            }
            else if (hit.collider.gameObject.GetComponent<Billboard>() != null)
            {
                Debug.Log("¡Tocaste un Billboard sin componente de interacción!");
            }
            else
            {
                Debug.Log("Objeto tocado sin componente IInteractable: " + hit.collider.gameObject.name);
            }
        }
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

    // Zoom methods ------------------------------------------------------------
    public void OnPinchStart(InputAction.CallbackContext context)
    {
        isZooming = true;
        isDragging = false;

        if (Touch.activeTouches.Count < 2) return;

        Touch primary = Touch.activeTouches[0];
        Touch secondary = Touch.activeTouches[1];

        if (primary.phase == TouchPhase.Moved || secondary.phase == TouchPhase.Moved)
        {
            if (primary.history.Count < 1 || secondary.history.Count < 1) return;
                
            float previousDistance = Vector2.Distance(primary.history[0].screenPosition, secondary.history[0].screenPosition);
            float currentDistance = Vector2.Distance(primary.screenPosition, secondary.screenPosition);
            float pinchDistance = previousDistance - currentDistance;

            Zoom(pinchDistance);
        }

    }

    private void OnPinchEnd(InputAction.CallbackContext context)
    {
        isZooming = false;
    }

    public void Zoom(float distance)
    {
        distance *= cameraZoomSpeed;
        targetOrthoSize = Mathf.Clamp(targetOrthoSize + distance, minOrthoSize, maxOrthoSize);

        UpdateCameraBounds();
    }

    private void UpdateCameraBounds()
    {
        if (targetContent != null)
        {
            SpriteRenderer sr = targetContent.GetComponent<SpriteRenderer>();
            Vector2 spriteSize = sr.bounds.size;
            float vertExtent = targetOrthoSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;

            minX = targetContent.position.x - spriteSize.x / 2 + horzExtent;
            maxX = targetContent.position.x + spriteSize.x / 2 - horzExtent;
            minY = targetContent.position.y - spriteSize.y / 2 + vertExtent;
            maxY = targetContent.position.y + spriteSize.y / 2 - vertExtent;
        }
    }
}
