using UnityEngine;

class EnemySpawner : PoolScript
{
    public Player m_Player = null;
    [Range(1, 100)]
    public int m_Chance = 1;
    public int m_WeightOfEnemy = 1;
    public int m_AmountofEnemies = 0;

    public void Spawn()
    { 
        GameObject zombie = getPooledObj();
        if (zombie != null)
        {
            zombie.transform.position = transform.position;
            zombie.transform.rotation = transform.rotation;
            zombie.SetActive(true);
            zombie.GetComponent<Enemy>().Target = m_Player;
            m_AmountofEnemies += m_WeightOfEnemy;
        }
    }

    public override void ResetPool()
    {
        base.ResetPool();
        m_AmountofEnemies = 0;
    }
}