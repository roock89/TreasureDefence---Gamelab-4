using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Enemy> enemies = new List<Enemy>();

    public int[] enemyPerWave;
    
    public PathController pathController;
    
    private int playerHealth;
    
    private int currentWave = 0;
    private int numWavesMax;

    private bool mapSetUp = false;

    private bool isWave;


    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        numWavesMax = enemyPerWave.Length;
    }

    public Vector3 GetPosOfEnemy(int index)
    {
        return enemies[index].GetPosition();
    }

    public float GetProgressOfEnemy(int index)
    {
        return enemies[index].GetProgress();
    }
    
    public void Setup(int maxWaves, int maxHealth)      // When player chooses a new map. Instead of ints we can give 
    {                                                   // maps a scriptableobject (or something) and pass several variables to the GM. Cleaner imo
        if (!mapSetUp)
        {
            currentWave = 0;
            numWavesMax = maxWaves;

            playerHealth = maxHealth;           // Should player health change based on map?

            mapSetUp = true;
        }
    }

    public void ResetMap()     // Called if player wins, loses, or gives up on a map
    {
        if (mapSetUp)
        {
            mapSetUp = false;
        }
    }

    public void StartWave()    // Glug o' rum
    {
        if (!isWave)
        {
            isWave = true;
            
            EnemySpawner.instance.StartWave(enemyPerWave[currentWave]);
            // Start spawning enemies. Something like EnemySpawner.Spawn(int wave);
        }
    }

    public void EndWave()
    {
        if (isWave)
        {
            isWave = false;

            currentWave++;

            if (currentWave >= numWavesMax)
            {
                EndMap(true); // Already know this is bugged cuz player might lose on the last round, after all enemies have been spawned.
            }
        }
    }

    private void EndMap(bool win)
    {
        if (win)
        {
            // Player wins
            Debug.LogWarning("You won! :)");
        }
        else
        {
            // Player loses
            Debug.LogWarning("You lost and you suck ass! :)");
        }
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            ResetMap();
            // I feel like we don't wanna just call this immediately. We have to let the player realize they lost, not just stop everything.
        }
    }

    public void AddEnemy(Enemy e)
    {
        e.path = pathController;
        
        enemies.Add(e);
    }

    public void RemoveEnemy(Enemy e)
    {
        enemies.Remove(e);
    }
    
    
}