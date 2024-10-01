using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class EnemySpawnManager : MonoBehaviour
{
    public Transform[] spawnPositions; // Array to hold spawn positions
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public float spawnRate = 2.0f; // Rate at which enemies are spawned (in seconds)
    public int maxEnemies = 10; // Maximum number of enemies allowed in the scene

    private int currentEnemyCount = 0; // Current number of enemies in the scene
    private int currentActiveEnemy = 0;


    public TMP_Text health_txt;

    private float health = 100;

    public PlayableDirector BossShot;
    public bool SpawnRoutine = false;
    private bool bossShotPlayed = false;

    public TimeLineManager timeLineManager;

    public Image healthFill;

    // Start is called before the first frame update
    void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnEnemies());
    }

    

    IEnumerator SpawnEnemies()
    {
        SpawnRoutine = true;
        while (currentEnemyCount < maxEnemies)
        {

            if (currentActiveEnemy < spawnRate) 
            {
                // Check if the current number of enemies is less than the maximum allowed
                // Choose a random spawn position from the array
                Transform spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];

                // Instantiate the enemy at the chosen position
                GameObject Enemy = Instantiate(enemyPrefab, spawnPosition.position, spawnPosition.rotation);
                EnemyFollow enemyFollow = Enemy.GetComponent<EnemyFollow>();

                if (enemyFollow != null)
                {
                    enemyFollow.enemySpawnManager = this;
                }

                currentActiveEnemy++;
                currentEnemyCount++;
            }
            

            // Increment the current enemy count
            
            

            // Wait for the specified spawn rate before spawning the next enemy
            yield return new WaitForSeconds(1f);
        }
        
        yield return StartCoroutine(CheckForRemainingEnemies());
    }

    IEnumerator CheckForRemainingEnemies()
    {
        while (!bossShotPlayed) // Ensure it only runs while the cutscene hasn't played yet
        {
            // Find all remaining enemies in the scene
            GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            // If no enemies remain, play the cutscene
            if (remainingEnemies.Length == 0)
            {
                Debug.Log("All enemies destroyed. Playing boss shot.");
                StartCoroutine(timeLineManager.PlayTimeLine(1));
                bossShotPlayed = true; // Mark that the cutscene has been played
            }

            // Wait for a short delay before checking again
            yield return new WaitForSeconds(1f);
        }

        SpawnRoutine = false; // Spawn routine finished
    }


    // Method to call when an enemy is destroyed (or deactivated)
    public void OnEnemyDestroyed()
    {
        currentActiveEnemy--;
    }

    public void healthUpdate(float dec)
    {
        if (health > 0)
        {
            health -= dec;
        }

        if (health < 0) health = 0;

        int healthint = (int) health;

        health_txt.text = healthint.ToString();
        healthFill.fillAmount =  (health) / 100;
        
    }

    public void setHealth(int h)
    {
        health = h;
        healthFill.fillAmount = health;
        health_txt.text = health.ToString();
    }
}
