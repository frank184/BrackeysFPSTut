using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float thrusterForce = 1000f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [SerializeField]
    private LayerMask environmentMask;

    // Component Cache
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    [Header("Spring Settings: ")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    public void Start()
    {
        motor    = GetComponent<PlayerMotor>();
        joint    = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        EnableJoint();
    }

    public void Update()
    {
        if (PauseMenu.IsOn) return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, environmentMask))
            joint.targetPosition = new Vector3(0f, -hit.point.y, 0f);
        else
            joint.targetPosition = new Vector3(0f, 0f, 0f);

        // Calculate movement velocity as a 3D vector
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");
        Vector3 moveHorizontal = transform.right * xMove; // (1, 0, 0)
        Vector3 moveVertical = transform.forward * zMove; // (0, 0, 1)
        Vector3 velocity = (moveHorizontal + moveVertical) * speed;
        motor.Move(velocity);

        // Animate Movement
        animator.SetFloat("ForwardVelocity", zMove);

        // Calculate player rotation as a 3D vector (turning around)
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 yRotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
        motor.Rotate(yRotation);

        // Calculate camera rotation as a 3D vector (turning around)
        float xRot = Input.GetAxisRaw("Mouse Y");
        float xRotation = xRot * lookSensitivity;
        motor.RotateCamera(xRotation);

        // Calculate thrusterForce Vector3
        Vector3 thrusterVector = Vector3.zero;
        if ( Input.GetButton("Jump") && thrusterFuelAmount > 0)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;
            if (thrusterFuelAmount >= 0.0f)
            {
                thrusterVector = Vector3.up * thrusterForce;
                DisableJoint();
            }
        }
        else
        {
            EnableJoint();
        }
        thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0, 1);

        // Apply thrusterForce
        motor.ApplyThruster(thrusterVector);
    }

    private void EnableJoint()
    {
        joint.yDrive = CreateJointSettings(jointSpring);
    }

    private void DisableJoint()
    {
        joint.yDrive = CreateJointSettings(0f);
    }

    private JointDrive CreateJointSettings(float springPosition)
    {
        return new JointDrive
        {
            positionSpring = springPosition,
            maximumForce = jointMaxForce
        };
    }
}
