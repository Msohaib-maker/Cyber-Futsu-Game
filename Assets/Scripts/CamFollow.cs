using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public Transform playerObj;

    [SerializeField] private Transform cameraRig; // Assign CameraRig GameObject here
    [SerializeField] private float sensitivityY = 200f;
    [SerializeField] private float angle_limit = 30f;

    private float xRotation = 0f;
    

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    void LateUpdate()
    {
        
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;


        // Rotate the camera independently along the X and Y axis
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -angle_limit, angle_limit); // Prevent over-rotation on Y axis (up/down)

        

        transform.rotation = Quaternion.Euler(xRotation, playerObj.transform.rotation.eulerAngles.y, 0);
        
        transform.position = cameraRig.position;

        
    }

    //public GameObject player;
    //[SerializeField] private Vector3 offset;
    //public float distance;

    //public float y_angle;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    offset = transform.position - player.gameObject.transform.position;
    //    distance = offset.magnitude;

    //    y_angle = transform.rotation.y;
    //}


    //private float Common_Code()
    //{
    //    float square_dis = distance * distance;

    //    float delta_y = player.transform.position.y - transform.position.y;



    //    float square_delta_y = delta_y * delta_y;


    //    square_dis = square_dis - square_delta_y;

    //    float dis = Mathf.Sqrt(square_dis);

    //    return dis;
    //}

    //private void Left_Mor()
    //{

    //    float dis = Common_Code();
    //    float x1 = player.transform.position.x + dis;
    //    transform.position = new Vector3(x1, transform.position.y, player.transform.position.z);
    //    transform.rotation = Quaternion.Euler(35, -90, 0);

    //}

    //private void Right_Mor()
    //{
    //    float dis = Common_Code();
    //    float x1 = player.transform.position.x - dis;
    //    transform.position = new Vector3(x1, transform.position.y, player.transform.position.z);
    //    transform.rotation = Quaternion.Euler(35, 90, 0);

    //}

    //private void Seedha_Mor()
    //{
    //    float dis = Common_Code();
    //    float z1 = player.transform.position.z - dis;
    //    transform.position = new Vector3(player.transform.position.x, transform.position.y, z1);
    //    transform.rotation = Quaternion.Euler(35, 0, 0);
    //}

    //private void Back_Mor()
    //{
    //    float dis = Common_Code();
    //    float z1 = player.transform.position.z + dis;
    //    transform.position = new Vector3(player.transform.position.x, transform.position.y, z1);
    //    transform.rotation = Quaternion.Euler(35, 180, 0);

    //}



    //public void CalculateUnKnown(string unknown)
    //{
    //    if (unknown == "-x") // Left
    //    {
    //        Left_Mor();
    //    }
    //    if (unknown == "x") // Right
    //    {
    //        Right_Mor();
    //    }
    //    if (unknown == "-z") // Seedha
    //    {
    //        Seedha_Mor();
    //    }
    //    if (unknown == "z") // back
    //    {
    //        Back_Mor();
    //    }

    //    offset = transform.position - player.gameObject.transform.position;

    //}

    //// Update is called once per frame
    //void LateUpdate()
    //{

    //    /*y_angle += Input.GetAxis("Mouse X") * 3f;
    //    y_angle = Mathf.Clamp(y_angle, -60, 60);

    //    transform.rotation = Quaternion.Euler(35, y_angle, 0);


    //    transform.position = player.transform.position + offset;*/
    //}
}
