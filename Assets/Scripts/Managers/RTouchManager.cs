using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.EventSystems;

/// <summary>
/// Manages touch input interactions for camera control and object interaction in a Unity scene.
/// Handles single-touch gestures for camera rotation, tap detection for object interaction,
/// and pinch gestures for camera zooming. Integrates with Unity's Input System and supports
/// raycasting to interactable objects and UI detection.
/// </summary>
/// <remarks>
/// Attach this component to a GameObject with a <see cref="PlayerInput"/> component.
/// Requires a configured Input Actions asset with actions: "TouchPress", "TouchPosition", "PinchFinger1", "PinchFinger2".
/// </remarks>
public class RTouchManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction pinchAction;

    private Camera mainCamera;
    private Transform cameraTransform;

    private Quaternion targetRotation;
    private float targetFOV;

    private PointerEventData pointerEventData;

    private Vector2 initialTouchPosition;
    private float touchStartTime;
    private bool isDragging;
    private bool isZooming;

    [SerializeField]
    private float distance = 50f; // Maximum distance for raycasting to detect interactable objects
    [SerializeField]
    private LayerMask mask; // Layer mask for raycasting

    [SerializeField]
    private float dragThreshold = 0.2f; // Threshold to distinguish between tap and drag
    [SerializeField]
    private float cameraRotationSpeed = 0.06f; // Adjust rotation speed as needed
    [SerializeField]
    private float cameraSmoothSpeed = 15f; // Speed of camera smoothing

    [SerializeField]
    private float cameraZoomSpeed = 15f; // Adjust camera zoom speed as needed
    [SerializeField]
    private float minFOV = 20f; // Minimum field of view for zooming
    [SerializeField]
    private float maxFOV = 100f; // Maximum field of view for zooming

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

        cameraTransform = mainCamera?.transform;
        targetRotation = cameraTransform.rotation;

        targetFOV = mainCamera.fieldOfView;
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
        // Smoothly rotates the camera towards the target rotation
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, Time.deltaTime * cameraSmoothSpeed);

        // Smoothly adjusts the camera's field of view for zooming
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * cameraSmoothSpeed);
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
        UpdateTargetRotation(currentTouchPosition - initialTouchPosition);
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

    // Camera control and rotation methods ------------------------------------------------------------
    private void UpdateTargetRotation(Vector2 delta)
    {
        // Adjust rotation speed based on camera's field of view
        float fovFactor = mainCamera.fieldOfView / 60f;
        float adjustedRotationSpeed = cameraRotationSpeed * fovFactor;

        // Get Current Rotation
        Vector3 angles = targetRotation.eulerAngles;

        // Horizontal Rotation (Y axis)
        angles.y += -delta.x * adjustedRotationSpeed;

        // Vertical Rotation (X axis)
        float currentXRotation = angles.x;
        if (currentXRotation > 180f) currentXRotation -= 360f;

        // Clamped between -80° and 80° to prevent the camera from flipping over
        float newXRotation = Mathf.Clamp(currentXRotation + delta.y * adjustedRotationSpeed, -80f, 80f);
        angles.x = newXRotation;

        // Apply New Rotation
        targetRotation = Quaternion.Euler(angles);
    }

    public void SetTargetRotation(Quaternion newRotation)
    {
        targetRotation = newRotation;
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
        targetFOV = Mathf.Clamp(mainCamera.fieldOfView + distance, minFOV, maxFOV);
    }


}
