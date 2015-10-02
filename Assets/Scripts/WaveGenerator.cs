using Assets.Scripts;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {
    protected WaveGenerator() { }

    public float toThePower = 1.5f;
    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 1.5f;
    public int spawnThreshold = 80;
    public int baseAmount = 10;

    private ZombieSpawner[] spawners;
    private int zombies;
    private int totalWeight;

    void Awake(){
        spawners = GetComponentsInChildren<ZombieSpawner>();

        for (int i = 0; i < spawners.Length; i++)
            totalWeight += spawners[i].chance;
    }

    void Start()
    {
        setLevel(1);
    }

    void Spawn()
    {
        if (zombies <= spawnedZombies()) return;
        if (totalActiveZombies() > spawnThreshold) {
            Invoke("Spawn", 0.5f);
            return;
        }

        int random = Random.Range(1, totalWeight+1);

        for (int i = 0; i < spawners.Length; i++){
            random -= spawners[i].chance;
            if (random <= 0)
            {
                spawners[i].Spawn();
                break;
            }
        }

        Invoke("Spawn", Random.Range(minSpawnTime, maxSpawnTime));
    }

    public void setLevel(int level)
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            ZombieSpawner spawner = spawners[i];
            spawner.ResetPool();
            zombies = baseAmount + (int) Mathf.Pow(level-1, toThePower);

            Invoke("Spawn", 5f);
        }
    }



    public int totalActiveZombies()
    {
        int amount = 0;
        for (int i = 0; i < spawners.Length; i++)
        {
            amount += spawners[i].getActiveObjects();
        }
        return amount;
    }

    public int spawnedZombies()
    {
        int amount = 0;
        for (int i = 0; i<spawners.Length; i++)
        {
            amount += spawners[i].amountOfZombies;
        }
        return amount;
    }







}
