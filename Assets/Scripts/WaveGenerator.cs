using System.Collections;
using UnityEngine;

public class WaveGenerator : MonoBehaviour{
    public float m_ToThePower = 1.5f;
    public float m_MinSpawnTime = 0.5f;
    public float m_MaxSpawnTime = 1.5f;
    public int m_SpawnThreshold = 80;
    public int m_BaseAmount = 10;

    private EnemySpawner[] m_Spawners;
    private int m_EnemiesToCompleteWave;
    private int m_TotalWeight;

    private IEnumerator m_SpawnRoutine;

    public int EnemiesToCompleteWave { get { return m_EnemiesToCompleteWave; } }
	public int EnemiesLeft {
		get { return m_EnemiesToCompleteWave - spawnedEnemies() + totalActiveEnemies(); }
	}
    
    void Awake(){

        m_Spawners = GetComponentsInChildren<EnemySpawner>();

        for (int i = 0; i < m_Spawners.Length; i++)
            m_TotalWeight += m_Spawners[i].m_Chance;

        m_SpawnRoutine = Spawn();

        setWave(5);
        StartSpawning();
    }

    public void StartSpawning()
    {
        StartCoroutine(m_SpawnRoutine);
    }
    public void StopSpawning()
    {
        StopCoroutine(m_SpawnRoutine);
    }

    IEnumerator Spawn()
    {
        while (true){

            if (spawnedEnemies() >= m_EnemiesToCompleteWave){
                StopSpawning();
                yield return null;
            }

            if (totalActiveEnemies() > m_SpawnThreshold)
                yield return new WaitForSeconds(Random.Range(m_MinSpawnTime, m_MaxSpawnTime));

            int random = Random.Range(1, m_TotalWeight + 1);

            for (int i = 0; i < m_Spawners.Length; i++)
            {
                random -= m_Spawners[i].m_Chance;
                if (random <= 0)
                {
                    m_Spawners[i].Spawn();
                    break;
                }
            }

            yield return new WaitForSeconds(Random.Range(m_MinSpawnTime, m_MaxSpawnTime));
        }
    }

    public void setWave(int level)
    {
        StopSpawning();
        for (int i = 0; i < m_Spawners.Length; i++)
        {
            EnemySpawner spawner = m_Spawners[i];
            spawner.ResetPool();
            m_EnemiesToCompleteWave = m_BaseAmount + (int) Mathf.Pow(level-1, m_ToThePower);
        }
    }

    public bool isWaveCompleted(){
        return m_EnemiesToCompleteWave <= spawnedEnemies() && totalActiveEnemies() == 0;
    }

    public int totalActiveEnemies()
    {
        int amount = 0;
        for (int i = 0; i < m_Spawners.Length; i++)
        {
            amount += m_Spawners[i].getActiveObjects();
        }
        return amount;
    }

    public int spawnedEnemies()
    {
        int amount = 0;
        for (int i = 0; i<m_Spawners.Length; i++)
        {
            amount += m_Spawners[i].m_AmountofEnemies;
        }
        return amount;
    }







}
