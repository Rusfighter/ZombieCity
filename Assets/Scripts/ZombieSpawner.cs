using UnityEngine;

namespace Assets.Scripts
{
    class ZombieSpawner : PoolScript
    {
        public Player player = null;
        [Range(1, 100)]
        public int chance = 1;
        public int WeightOfZombie = 1;
        public int amountOfZombies = 0;

        public void Spawn()
        { 
            GameObject zombie = getPooledObj();
            if (zombie != null)
            {
                zombie.transform.position = transform.position;
                zombie.transform.rotation = transform.rotation;
                zombie.SetActive(true);
                zombie.GetComponent<Enemy>().Target = player;
                amountOfZombies += WeightOfZombie;
            }
        }

        public override void ResetPool()
        {
            base.ResetPool();
            amountOfZombies = 0;
        }
    }
}
