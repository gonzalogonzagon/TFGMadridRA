using UnityEngine;

public class ContentRotationController : MonoBehaviour
{
    [SerializeField] private GameObject contentRoot;
    [SerializeField] private float rotationSpeed = 60f; // Degrees per second for continuous rotation
    private bool isRotating = false;

    // Called by the button on press
    public void RotateStep()
    {
        contentRoot.transform.Rotate(0, 5f, 0, Space.World);
    }

    // Called by the button on hold (OnPointerDown)
    public void StartContinuousRotation()
    {
        isRotating = true;
    }

    // Called by the button on release (OnPointerUp)
    public void StopContinuousRotation()
    {
        isRotating = false;
    }

    void Update()
    {
        if (isRotating)
        {
            contentRoot.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }
    }
}
