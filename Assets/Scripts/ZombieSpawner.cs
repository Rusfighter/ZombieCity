using UnityEngine;

namespace Assets.Scripts
{
    class ZombieSpawner : MonoBehaviour
    {

        public Player player = null;

        public float minSpawn = 3;
        public float maxSpawn = 10;

        PoolScript[] pools;

        void Start()
        {
            pools = GetComponents<PoolScript>();
            Spawn();
        }

        public void Spawn()
        {
            if (pools == null && pools.Length < 1) return;
            PoolScript randomPool = pools[Random.Range(0, pools.Length)];

            GameObject zombie = randomPool.getPooledObj();
            if (zombie != null)
            {
                zombie.SetActive(true);
                zombie.transform.position = transform.position;
                zombie.transform.rotation = transform.rotation;
                zombie.GetComponent<Enemy>().Player = player;
            }

            Invoke("Spawn", Random.Range(minSpawn, maxSpawn));
        }


    }
}
