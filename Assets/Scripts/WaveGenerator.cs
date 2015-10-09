using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {

    public static WaveGenerator instance;

    public float toThePower = 1.5f;
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 1.5f;
    public int spawnThreshold = 80;
    public int baseAmount = 10;

    private EnemySpawner[] spawners;
    private int enemiesToCompleteWave;
    private int totalWeight;

    public int EnemiesToCompleteWave { get { return enemiesToCompleteWave; } }
	public int EnemiesLeft {
		get { return enemiesToCompleteWave - spawnedEnemies() + totalActiveEnemies(); }
	}
    private IEnumerator spawnRoutine;

    void Awake(){
        if (instance == null)
        {
            if (FindObjectsOfType(GetType()).Length > 1) {
                Debug.LogError("To many instances of "+ GetType());
                return;
            } else instance = this;
        }

        spawners = GetComponentsInChildren<EnemySpawner>();

        for (int i = 0; i < spawners.Length; i++)
            totalWeight += spawners[i].chance;

        spawnRoutine = Spawn();
    }

    public void StartSpawning()
    {
        StartCoroutine(spawnRoutine);
    }
    public void StopSpawning()
    {
        StopCoroutine(spawnRoutine);
    }

    IEnumerator Spawn()
    {
        while (true){
            if (totalActiveEnemies() > spawnThreshold)
                yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));

            int random = Random.Range(1, totalWeight + 1);

            for (int i = 0; i < spawners.Length; i++)
            {
                random -= spawners[i].chance;
                if (random <= 0)
                {
                    spawners[i].Spawn();
                    break;
                }
            }

            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }

    public void setWave(int level)
    {
        StopSpawning();
        for (int i = 0; i < spawners.Length; i++)
        {
            EnemySpawner spawner = spawners[i];
            spawner.ResetPool();
            enemiesToCompleteWave = baseAmount + (int) Mathf.Pow(level-1, toThePower);
        }
    }

    public bool isWaveCompleted(){
        return enemiesToCompleteWave <= spawnedEnemies() && totalActiveEnemies() == 0;
    }

    public int totalActiveEnemies()
    {
        int amount = 0;
        for (int i = 0; i < spawners.Length; i++)
        {
            amount += spawners[i].getActiveObjects();
        }
        return amount;
    }

    public int spawnedEnemies()
    {
        int amount = 0;
        for (int i = 0; i<spawners.Length; i++)
        {
            amount += spawners[i].amountOfEnemies;
        }
        return amount;
    }







}
