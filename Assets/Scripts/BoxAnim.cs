using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxAnim : MonoBehaviour
{
    private float y_angle = 0f;
    public float speed = 0.1f;

    public int bullets_fill = 70;

    public float ReappearTime = 10f;
    private float ReappearTimeLeft;

    private GunController gunControlScript;
    public GameObject gun;


    private MeshRenderer indicatorRenderer;
    public Material InvisibleMat;
    private Material OriginalMat;

    public TMP_Text time;



    private void Start()
    {
        ReappearTimeLeft = ReappearTime;
        //GameObject gun = GameObject.Find("camera/SciFiGunLightBlue");

        indicatorRenderer = GetComponent<MeshRenderer>();
        OriginalMat = indicatorRenderer.material;
       

        time.text = "00 : 00";

        gunControlScript = gun.GetComponent<GunController>();

        if (gunControlScript != null)
        {
            //Debug.Log("GunController script found and assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        y_angle += speed;
        transform.rotation = Quaternion.Euler(0f, y_angle, 0f);
        if (ReappearTime < ReappearTimeLeft)
        {
            ReappearTime += Time.deltaTime;
            if (ReappearTime>= ReappearTimeLeft)
            {
                indicatorRenderer.material = OriginalMat;
                ReappearTime = ReappearTimeLeft;
            }
        }

        UpdateTimeText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (ReappearTime >= ReappearTimeLeft)
            {
                ReappearTime = 0;
                gunControlScript.refillAmmo(bullets_fill);

                indicatorRenderer.material = InvisibleMat;
            }
        }
    }

    void UpdateTimeText()
    {
        // Calculate minutes and seconds from ReappearTime

        float DeltaAppearTime = (ReappearTimeLeft - ReappearTime);
        int minutes = Mathf.FloorToInt(DeltaAppearTime / 60f);
        int seconds = Mathf.FloorToInt(DeltaAppearTime % 60f);

        // Format the time as "MM : SS"
        time.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
