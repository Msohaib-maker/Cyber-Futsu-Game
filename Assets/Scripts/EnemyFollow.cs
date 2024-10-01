using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyFollow : MonoBehaviour
{
    private Transform transformToFollow;
    // NavMesh Agent variable
    NavMeshAgent agent;
    public GameObject bulletprefab;
    public Transform firePoint;

    private enum EnemyState { Moving, Attacking };
    //private EnemyState currentState;

    public float shootingDistance = 70f;
    public float attackDistance = 5.0f;
    public float bulletSpeed = 20f;
    public float AttackFrequency = 1.5f;

    public Material hitMaterial; // Assign the "hit" material in the inspector
    private Material originalMaterial; // To store the original material
    private SkinnedMeshRenderer meshRenderer; // Reference to the enemy's 3D mesh renderer


    public Image healthbar;
    public int DamageHits = 3;
    public int HitsTaken = 0;

    public float EnemyDamagedDuration = 0.5f;

    private float TimeDelay = 0f;
    private Animator enemyanim;

    public EnemySpawnManager enemySpawnManager;

    // Reference to the health bar's Canvas
    public Canvas healthBarCanvas;

    // Reference to the camera (Optional: you can directly assign Camera.main in Start)
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (healthBarCanvas != null)
        {
            //Debug.Log("Canvas assigned in enemy");
            mainCamera = Camera.main; // Get the main camera in the scene
            healthBarCanvas.worldCamera = mainCamera; // Assign it to the Canvas
        }

        agent = GetComponent<NavMeshAgent>();
        enemyanim = GetComponent<Animator>();


        // Find the player by tag and get its transform
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            transformToFollow = player.transform;
        }
        else
        {
            Debug.LogWarning("Player with tag 'Player' not found!");
        }

        // Find the MeshRenderer of the child (3D mesh) and store the original material
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
            Debug.Log("Mesh assigned");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (transformToFollow != null)
        {

            float distanceToPlayer = Vector3.Distance(transform.position, transformToFollow.position);

            if (TimeDelay >= AttackFrequency)
            {
                Attack();
                TimeDelay = 0f;
            }
            TimeDelay += Time.deltaTime;

            // Check if the enemy is within attack distance
            if (distanceToPlayer <= attackDistance)
            {
                // Switch to Attacking state
                
                agent.isStopped = true; // Stop the enemy movement
                enemyanim.SetTrigger("GoAttack"); // Trigger attack animation
                
            }
            else
            {
                // Switch to Moving state if the enemy is far enough from the player
                
                agent.isStopped = false; // Resume movement
                agent.destination = transformToFollow.position; // Move toward player
                enemyanim.SetTrigger("GoMove"); // Trigger movement animation
            }
        }
    }

    private void Attack()
    {
        // Calculate direction from the firePoint to the player
        Vector3 directionToPlayer = (transformToFollow.position - firePoint.position).normalized;

        // Perform a raycast to check if the player is directly in front of the enemy
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, directionToPlayer, out hit, shootingDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Check if the enemy is facing the player within a certain angle
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                if (angleToPlayer < 80f) // Adjust this angle as needed
                {
                    // Instantiate bullet and shoot
                    GameObject bullet = Instantiate(bulletprefab, firePoint.position, firePoint.rotation);
                    EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();

                    // Set the enemy's collider to be ignored by the bullet
                    bulletScript.SetShooter(GetComponent<Collider>());
                    bulletScript.enemySpawnManager = enemySpawnManager;

                    Rigidbody rb = bullet.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = directionToPlayer * bulletSpeed;
                    }
                    else
                    {
                        Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
                    }

                    Destroy(bullet, 5f);
                }
            }
        }
    }

    public void TakeDamage()
    {

        if (meshRenderer != null)
        {
            Debug.Log("Take Damage");
            // Change material to the "hit" material
            meshRenderer.material = hitMaterial;
            // Revert back to the original material after a delay
            StartCoroutine(ResetMaterialAfterTime(EnemyDamagedDuration)); // Revert after 0.5 seconds
        }
        HitsTaken++;

        healthbar.fillAmount = (float)(DamageHits - HitsTaken) / DamageHits;

    }

    private IEnumerator ResetMaterialAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Revert back to the original material
        meshRenderer.material = originalMaterial;
    }


    private void OnDestroy()
    {
        enemySpawnManager.OnEnemyDestroyed();
    }

    //private void Attack()
    //{
    //    GameObject bullet = Instantiate(bulletprefab, firePoint.position, firePoint.rotation);

    //    EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();



    //    // Set the enemy's collider to be ignored by the bullet
    //    bulletScript.SetShooter(GetComponent<Collider>());

    //    bulletScript.enemySpawnManager = enemySpawnManager;


    //    // Calculate direction from the firePoint to the player
    //    Vector3 directionToPlayer = (transformToFollow.position - firePoint.position).normalized;


    //    Rigidbody rb = bullet.GetComponent<Rigidbody>();
    //    if (rb != null)
    //    {
    //        rb.velocity = directionToPlayer * bulletSpeed;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Bullet prefab does not have a Rigidbody component.");
    //    }

    //    Destroy(bullet, 5f);
    //}
}
