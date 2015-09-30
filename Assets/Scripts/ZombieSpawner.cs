using UnityEngine;

namespace Assets.Scripts
{
    class ZombieSpawner : PoolScript
    {

        public Player player = null;

        public float minSpawn = 3;
        public float maxSpawn = 10;

        public int amountOfZombies = 0;

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
                zombie.GetComponent<Enemy>().Target = player;
                amountOfZombies++;
            }

            Invoke("Spawn", Random.Range(minSpawn, maxSpawn));
        }

        void Update()
        {
            int totalObjects = pooledObjects.Count;
            float divider = Mathf.Ceil(pooledObjects.Count / 15f);
            int remainder = Time.frameCount % (int)divider;
            int from = (int)Mathf.Lerp(0, totalObjects, remainder / divider);
            int to = (int)Mathf.Lerp(0, totalObjects, (1 + remainder) / divider);
            for (int i = from; i<to; i++)
            {
                GameObject obj = pooledObjects[i];
                if (obj.activeInHierarchy)
                {
                    obj.GetComponent<Enemy>().SlowUpdate();
                }
            }
        }
    }
}
