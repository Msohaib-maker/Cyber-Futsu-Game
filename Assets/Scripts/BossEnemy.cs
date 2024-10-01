using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class BossEnemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;

    public AudioSource laserbeamSFX;

    public float laserDamage = 0.005f; // Damage per second
    public float laserDuration = 1f; // How long the laser lasts

    private bool isFiringLaser = true;
    [SerializeField]private float laserCooldown = 6f; // Time between each laser shot
    [SerializeField] private float laserCooldownTimer = 6f;

    public LineRenderer laserLine;

    public float maxLaserDistance = 500f;

    public Transform LaserPos;

    public GameObject LaserParticlePrefab;

    //public TMP_Text healthboss;
    //public TMP_Text healthplayer;
    [SerializeField] private float bossHealth = 100f;

    public float DamageBoss = 0.9f;

    public Transform[] Crackpoints;
    public GameObject CrackPrefab;

    public PlayableDirector BossShot;

    public EnemySpawnManager EnemySpawnManagerCS;

    public Image BossFill;

    public GameObject FireExplodePrefab;

    private Animator animator;

    public GameObject[] GlowObjs;
    public Material NewMat;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        agent = GetComponent<NavMeshAgent>();

        

    }

    void GreyEyeBall()
    {
        foreach (var item in GlowObjs) 
        {
            MeshRenderer EyeBallMeshRenderer = item.GetComponent<MeshRenderer>();
            EyeBallMeshRenderer.material = NewMat;
        }
        
    }

    
    bool ObjectLose = false;

    void Update()
    {
        if (BossShot.state != PlayState.Playing)
        {
            BossAllPalyableLogic();
        }
        else
        {
            if (animator.enabled)
            {
                animator.enabled = false;
            }
        }

    }

    public float pushBackDistance = 1.0f;
    public void PushBack(Vector3 direction)
    {
        if (bossHealth > 0)
        {
            direction.Normalize();

            transform.position += direction * pushBackDistance;
        }
        
    }

    public float DeactivateTime = 30f;

    void BossAllPalyableLogic()
    {
        if (bossHealth > 0)
        {

            // Animation

            if (!animator.enabled)
            {
                animator.enabled = true;
            }

            // Move
            if (Vector3.Distance(transform.position, player.transform.position) > agent.stoppingDistance)
            {
                BossMove();  // Call the movement function
            }

            //Attacks
            if (laserCooldown >= laserCooldownTimer)
            {
                StartCoroutine(FireLaser());
                laserCooldown = 0;
            }

            laserCooldown += Time.deltaTime;
        }
        else
        {
            if (!agent.isStopped)
            {

                GreyEyeBall();
                animator.enabled = false;
                agent.isStopped = true;
                if (isFiringLaser)
                {
                    StopCoroutine(FireLaser());
                }

                if (!ObjectLose)
                {
                    Debug.Log("Boss Deafeated");
                    ObjectLose = true;
                    for (int i = 0; i < Crackpoints.Length; i++)
                    {
                        GameObject fireparticle = Instantiate(CrackPrefab, Crackpoints[i].position, Quaternion.identity);

                        Destroy(fireparticle, DeactivateTime);
                    }
                }


                StartCoroutine(Deactivate());
                
            }
            

        }
    }

    IEnumerator Deactivate()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(DeactivateTime);

        // Deactivate the GameObject
        gameObject.SetActive(false);
    }

    public void UpdateBoss()
    {
        bossHealth -= DamageBoss;
        if (BossFill != null)
        {
            if (bossHealth < 0) bossHealth = 0;
            BossFill.fillAmount = bossHealth/100;
            
        }
        
    }

    private void BossMove()
    {
        agent.destination = player.position;
    }

   

    IEnumerator FireLaser()
    {
        // First, calculate the distance to the player using a raycast
        float timer = 0;
        isFiringLaser = true;

        GameObject particleinstance = null;

        if (LaserParticlePrefab != null)
        {
            particleinstance = Instantiate(LaserParticlePrefab, LaserPos.position, Quaternion.identity);
            
        }

        yield return new WaitForSeconds(0.4f);


        laserbeamSFX.Play();
        while (timer < laserDuration)
        {
            timer += Time.deltaTime;

            
            Vector3 origin = LaserPos.position; // The starting point of the ray
            Vector3 direction = (player.position - origin).normalized; // Direction towards the player

            particleinstance.transform.position = origin;

            RaycastHit hit;

            // Cast the ray
            if (Physics.Raycast(origin, direction, out hit, maxLaserDistance))
            {


                if (hit.transform == player)
                {
                    EnemySpawnManagerCS.healthUpdate(laserDamage);
                  
                }

                // Update the Line Renderer positions
                laserLine.SetPosition(0, origin);
                laserLine.SetPosition(1, hit.point); // Update to the current hit point


                // Enable the laser line renderer
                laserLine.enabled = true;

            }
            else
            {
                // If no hit, set the laser to max distance
                laserLine.SetPosition(0, origin);
                laserLine.SetPosition(1, origin + direction * maxLaserDistance);

                // Enable the laser line renderer
                laserLine.enabled = true;
            }
            

            yield return null; // Wait for the next frame
        }

        if (particleinstance != null)
        {
            Destroy(particleinstance);
        }
        
        // Disable the laser after the duration
        laserLine.enabled = false;
        isFiringLaser = false;

        laserbeamSFX.Stop();
    }

}
