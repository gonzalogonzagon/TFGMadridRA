using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Reference to the main camera's transform; assigned in Start() and updated in LateUpdate() if lost
    private Transform cameraTransform;

    [SerializeField] private bool flipFace = false; // If true, flips the billboard to face away from the camera (useful for certain effects)
    [SerializeField] private bool useWorldUp = false; // Use world up instead of camera up

    // Start is called before the first frame update
    void Start()
    {
        FindMainCamera();
    }

    private void FindMainCamera()
    {
        cameraTransform = Camera.main?.transform;
    }

    // Use LateUpdate instead of Update to avoid jittering and ensure the camera has moved first
    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            // Calculate direction from the billboard to the camera
            Vector3 direction = cameraTransform.position - transform.position;
            // Use world up (Vector3.up) to keep the billboard upright globally, or camera up to match the camera's orientation.
            Vector3 up = useWorldUp ? Vector3.up : cameraTransform.up;

            // Look at the camera, flipping if needed
            transform.rotation = Quaternion.LookRotation(flipFace ? -direction : direction, up);

        } else
        {
            FindMainCamera();
        }
    }
}
