using UnityEngine;

namespace Assets.Scripts
{
    class EnemySpawner : PoolScript
    {
        public Player player = null;
        [Range(1, 100)]
        public int chance = 1;
        public int WeightOfEnemy = 1;
        public int amountOfEnemies = 0;

        public void Spawn()
        { 
            GameObject zombie = getPooledObj();
            if (zombie != null)
            {
                zombie.transform.position = transform.position;
                zombie.transform.rotation = transform.rotation;
                zombie.SetActive(true);
                zombie.GetComponent<Enemy>().Target = player;
                amountOfEnemies += WeightOfEnemy;
            }
        }

        public override void ResetPool()
        {
            base.ResetPool();
            amountOfEnemies = 0;
        }
    }
}
