using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet to be instantiated
    public Transform firePoint; // The point from where the bullet will be fired
    public float fireRate = 0.5f; // Time between shots
    private float nextTimeToFire = 0f;
    public ParticleSystem muzzleFlash; // Reference to the particle system

    private Camera mainCamera;

    public AudioSource FireSFX;

    private int kills = 0;
    public int bullets = 35;
    


    public TMP_Text bullets_txt;
    public TMP_Text kills_txt;
    public TMP_Text total_bullets_txt;

    public Image KillFill;


    public float x_angle;
    public float bulletSpeed = 20f;


    public int killTarget = 10;


    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found. Please tag your camera as 'MainCamera'.");
        }

        bullets_txt.text = bullets.ToString();

        kills_txt.text = kills.ToString();
        total_bullets_txt.text = bullets.ToString();

        
    }

    void Update()
    {

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && bullets > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                nextTimeToFire = Time.time + fireRate;
                update_amun_text();
                Fire();
            }
            
        }
        
    }

    void update_amun_text()
    {
        bullets--;
        bullets_txt.text = bullets.ToString();
    }

    public GameObject bossObj;

    void Fire()
    {

        FireSFX.Play();



        // Perform raycast from the center of the screen
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of the screen
        RaycastHit hit;
        Vector3 direction;

        // Debug: Visualize the raycast in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green, 1f);

        if (Physics.Raycast(ray, out hit))
        {
            direction = (hit.point - firePoint.position).normalized;
            //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
        }
        else
        {
            direction = ray.direction;
            //Debug.Log("Raycast did not hit any object.");
        }

        // Instantiate the bullet at the fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();


        bullet.GetComponent<PlayerBullet>().gunController = this;
        bullet.GetComponent<PlayerBullet>().bossEnemyCs = bossObj.GetComponent<BossEnemy>();

        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
        }

        // Instantiate and play the muzzle flash effect
        ParticleSystem muzzleFlashInstance = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
        muzzleFlashInstance.Play();
        Destroy(muzzleFlashInstance.gameObject, muzzleFlashInstance.main.duration);
    }

    public void refillAmmo(int refill)
    {
        bullets = refill;
        bullets_txt.text = bullets.ToString();
    }


    public void killsUpdate()
    {
        kills += 1;
        kills_txt.text = kills.ToString();
        KillFill.fillAmount = (float)kills / killTarget;
    }


    

}
