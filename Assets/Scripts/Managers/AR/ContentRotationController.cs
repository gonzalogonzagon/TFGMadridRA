using UnityEngine;

public class ContentRotationController : MonoBehaviour
{
    [SerializeField] private GameObject contentRoot;
    [SerializeField] private float rotationSpeed = 60f; // Degrees per second for continuous rotation
    private bool isRotating = false;

    private bool CanRotate() => contentRoot != null && contentRoot.activeInHierarchy;

    // Called by the button on hold (OnPointerDown)
    public void StartContinuousRotation()
    {
        if (CanRotate())
            isRotating = true;
    }

    // Called by the button on release (OnPointerUp)
    public void StopContinuousRotation()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating && CanRotate())
        {
            contentRoot.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }
    }
}
