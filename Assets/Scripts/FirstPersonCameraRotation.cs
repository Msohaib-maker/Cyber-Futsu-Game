using UnityEngine;

/// <summary>
/// A simple FPP (First Person Perspective) camera rotation script.
/// Like those found in most FPS (First Person Shooter) games.
/// </summary>
public class FirstPersonCameraRotation : MonoBehaviour
{

    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [SerializeField] private float moveSpeed = 5f;  // Movement speed

    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0f;

    private Rigidbody rb;
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";


    public float lower_limit = 0.1f;
    public float upper_limit = 0.3f;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat; //Quaternions seem to rotate more consistently than EulerAngles. Sensitivity seemed to change slightly at certain degrees using Euler. transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        //transform.Translate(Input.GetAxis("Horizontal") * .2f, 0, Input.GetAxis("Vertical") * .2f);
        
    }


    private void FixedUpdate()
    {
        // Handle movement using Rigidbody
        Vector3 moveDirection = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        // Raycasting to detect steps
        //RaycastHit hit;
        //Vector3 origin = transform.position + Vector3.up * 0.1f;  // Slightly above the ground
        //Vector3 direction = transform.forward;

        //if (Physics.Raycast(origin, direction, out hit, stepRayLength))
        //{
        //    Debug.Log("Step detected : " + hit.normal.y);

        //    if (hit.normal.y < 0.1f) // Detect a vertical surface
        //    {
        //        Debug.Log("Jump");
        //        // Adjust position upwards to "step" onto the obstacle
        //        rb.position += Vector3.up * stepHeight;
        //    }
        //}

        stepClimb();
    }

    void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, lower_limit))
        {
            RaycastHit hitUpper;
            Debug.Log("lower 0");
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, upper_limit))
            {
                Debug.Log("upper 0");
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
                Debug.Log(rb.position);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, lower_limit))
        {

            RaycastHit hitUpper45;
            Debug.Log("lower 45");
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, upper_limit))
            {
                Debug.Log("upper 45");
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
                Debug.Log(rb.position);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, lower_limit))
        {
            Debug.Log("lower -45");
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, upper_limit))
            {
                Debug.Log("upper -45");
                rb.position -= new Vector3(0f, -stepSmooth , 0f);
                Debug.Log(rb.position);
            }
        }
    }
}