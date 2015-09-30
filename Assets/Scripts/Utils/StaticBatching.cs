using UnityEngine;
using System.Collections.Generic;

public class StaticBatching : MonoBehaviour {
    void Awake()
    {
        int layerid = LayerMask.NameToLayer("Static");
        List<GameObject> gameobjs = new List<GameObject>();

        MeshRenderer[] objs = FindObjectsOfType<MeshRenderer>();
        for (int i = 0; i<objs.Length; i++)
        {
            GameObject obj = objs[i].gameObject;
            if (obj.layer == layerid)
            {
                gameobjs.Add(obj);
                //obj.SetActive(false);
            }
        }
        StaticBatchingUtility.Combine(gameobjs.ToArray(), gameObject);
    }
}
