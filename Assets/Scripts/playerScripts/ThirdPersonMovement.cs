using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController character_controller;
    public Transform camera_transform;
    public Rigidbody rigid_body;
    public Animator animator;
    public CombatScript combatScript;

    private float current_rotation_velocity;

    [Header("Player Settings")]
    public int max_health = 100;
    public float movement_speed = 12f;
    public float rotation_smooth_time = 0.1f;

    private int current_health;

    void Start()
    {
        if (rigid_body == null)
        {
            rigid_body = GetComponent<Rigidbody>();
        }

        current_health = max_health;
        combatScript = GetComponent<CombatScript>();
    }

    void Update()
    {
        ProcessPlayerMovement();
        combatScript.ProcessPlayerInput();
    }

    private void ProcessPlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float target_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera_transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_angle, ref current_rotation_velocity, rotation_smooth_time);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 move_dir = Quaternion.Euler(0f, target_angle, 0f) * Vector3.forward;
            character_controller.Move(move_dir.normalized * movement_speed * Time.deltaTime);
        }
        else
        {
            character_controller.Move(Vector3.zero);
        }
    }
}
