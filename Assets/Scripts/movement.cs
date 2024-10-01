using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float cameraSmoothSpeed = 0.1f; // Smooth speed for the camera

    public float y_angle;
    public float x_angle;
    public Animator anim;
    //public Transform cam; // Assign your camera here in the inspector
    //public Transform gun;
    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector3 cameraTargetPosition; // Target position for camera smoothing

    public float angle_limit = 5;

    void Start()
    {
        //anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        //cameraTargetPosition = cam.localPosition;
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        y_angle += Input.GetAxis("Mouse X");

        //Debug.Log(transform.position);

        transform.rotation = Quaternion.Euler(0, y_angle, 0);
        //Debug.Log("Player y : " + y_angle);
        //cam.localRotation = Quaternion.Euler(x_angle, 0, 0); // Rotate the camera up/down

        


        Vector3 moveDirection = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        characterController.Move(moveDirection * speed);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (anim != null)
        {
            bool ismove = moveDirection.magnitude > 0;
            anim.SetBool("iswalk", ismove);
        }
        // Smooth the camera's movement
        
    }
}
