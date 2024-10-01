using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFollow : MonoBehaviour
{

    public Rigidbody rb;
    public float speed = 7f;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.J))
            rb.MovePosition(transform.position + new Vector3(1, 0, 0) * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.L))
            rb.MovePosition(transform.position + new Vector3(-1, 0, 0) * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.I))
            rb.MovePosition(transform.position + new Vector3(0, 0, 1) * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.K))
            rb.MovePosition(transform.position + new Vector3(0, 0, -1) * speed * Time.deltaTime);
    }
}
