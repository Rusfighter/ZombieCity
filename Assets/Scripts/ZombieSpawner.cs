using UnityEngine;

namespace Assets.Scripts
{
    class ZombieSpawner : PoolScript
    {

        public Player player = null;

        public float minSpawn = 3;
        public float maxSpawn = 10;

        void Start()
        {
            Spawn();
        }

        public void Spawn()
        { 
            GameObject zombie = getPooledObj();
            if (zombie != null)
            {
                zombie.transform.position = transform.position;
                zombie.transform.rotation = transform.rotation;
                zombie.SetActive(true);
                zombie.GetComponent<Enemy>().Player = player;
            }

            Invoke("Spawn", Random.Range(minSpawn, maxSpawn));
        }
    }
}
