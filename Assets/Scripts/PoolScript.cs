using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class PoolScript : MonoBehaviour
    {
        public GameObject pooledObject = null;
        public int pooledAmount;
        public bool ableToGrow = true;

        protected List<GameObject> pooledObjects;

        void Awake() {
            pooledObjects = new List<GameObject>();
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject obj = createObject();
                pooledObjects.Add(obj);
            }
        }

        public GameObject getPooledObj()
        {
            if (pooledObject == null) return null;

            for (int i = 0; i < pooledObjects.Count; i++)
            {
                if (!pooledObjects[i].activeInHierarchy)
                {
                    return pooledObjects[i];
                }
            }

            if (ableToGrow)
            {
                GameObject obj = createObject();
                pooledObjects.Add(obj);
                pooledAmount++;
                return obj;
            }

            return null;
        }

        private GameObject createObject()
        {
            GameObject obj = Instantiate(pooledObject);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            return obj;
        }
    }
}
