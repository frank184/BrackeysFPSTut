using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {
    [SerializeField]
    private Camera cam;

    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 playerRotation = Vector3.zero;
    private float cameraXRotation = 0f;
    private float currentCameraXRotation = 0f;
    private Vector3 thrusterForce  = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85f;
    
    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 velocity)
    {
        playerVelocity = velocity;
    }

    public void Rotate(Vector3 rotation)
    {
        playerRotation = rotation;
    }

    public void RotateCamera(float xRotation)
    {
        cameraXRotation = xRotation;
    }

    public void ApplyThruster(Vector3 thruster)
    {
        thrusterForce = thruster;
    }

    // Run every Physics iteration
    public void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        if (playerVelocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + playerVelocity * Time.fixedDeltaTime);
        }

        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void PerformRotation()
    {
        if (playerRotation != Vector3.zero)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(playerRotation));
        }

        if (cam != null)
        {
            // Set our rotation and clamp it
            currentCameraXRotation += cameraXRotation;
            currentCameraXRotation = Mathf.Clamp(currentCameraXRotation, -cameraRotationLimit, cameraRotationLimit);
            // Apply rotation to camera's transform
            cam.transform.localEulerAngles = new Vector3( -currentCameraXRotation, 0f, 0f);
        }
    }
}
