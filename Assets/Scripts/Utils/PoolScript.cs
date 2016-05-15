using System.Collections.Generic;
using UnityEngine;

class PoolScript : MonoBehaviour
{
    public GameObject m_PooledObject = null;
    public int m_PooledAmount;
    public bool m_AbleToGrow = true;

    protected List<GameObject> pooledObjects;

    void Awake() {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < m_PooledAmount; i++)
        {
            GameObject obj = createObject();
            pooledObjects.Add(obj);
        }
    }

    public virtual void ResetPool()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(false);
            }
        }
    }

    public int getActiveObjects()
    {
        int amount = 0;
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy){
                amount++;
            }
        }
        return amount;
    }

    public GameObject getPooledObj()
    {
        if (m_PooledObject == null) return null;

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (m_AbleToGrow)
        {
            GameObject obj = createObject();
            pooledObjects.Add(obj);
            m_PooledAmount++;
            return obj;
        }

        return null;
    }

    private GameObject createObject()
    {
        GameObject obj = Instantiate(m_PooledObject);
        obj.transform.SetParent(transform);
        obj.SetActive(false);
        return obj;
    }
}
