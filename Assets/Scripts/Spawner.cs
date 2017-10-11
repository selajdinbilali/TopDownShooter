using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// in charge of spawning enemies
public class Spawner : MonoBehaviour {

    // the object Wave control the waves of enemies
    // we create an array to manage that
    public Wave[] waves;
    public Enemy enemy;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    Wave currentWave;
    int currentWaveNumber;

    void Start()
    {
        NextWave();
    }

    void Update()
    {
        // if stilly enemies and interval good
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            // decrement the conuter
            enemiesRemainingToSpawn--;
            // adjust the time
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
            // spawn the enemy
            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            // subscribe to the event OnDeath from the object Enemy inherited from LivingEntity
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    // this method is called when an enemy is dead

    void OnEnemyDeath()
    {
        // if an enemy is killed
        enemiesRemainingAlive--;
        // if no more enemies in this wave go next wave
        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        // wave starts at 1
        currentWaveNumber++;
        // if still waves
        if (currentWaveNumber - 1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];
            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
        
    }

    // to show the class in unity's editor
    [System.Serializable]
    public class Wave
    {
        // defined in the editor
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
