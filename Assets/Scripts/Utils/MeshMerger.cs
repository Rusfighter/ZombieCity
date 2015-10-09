#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class SingleBatch
{
    public Material mat;
    public List<CombineInstance> combines = new List<CombineInstance>();
}

public class MeshMerger : MonoBehaviour
{
    List<SingleBatch> batches = new List<SingleBatch>();

    void Awake()
    {
        Split(4,4);
    }

    void Split(int xgrids = 5, int ygrids = 5)
    {
        GameObject[,] grids = new GameObject[xgrids, ygrids];
        MeshFilter[] meshFilters = FindObjectsOfType<MeshFilter>();

        float minX = 0;
        float maxX = 0;
        float minY = 0;
        float maxY = 0;

        for (int i = 0; i < meshFilters.Length; i++)
        {
            GameObject obj = meshFilters[i].gameObject;
            if (!isStatic(obj)) continue;
            Vector3 pos = obj.transform.position;
            if (pos.x < minX) minX = pos.x;
            if (pos.x > maxX) maxX = pos.x;
            if (pos.z < minY) minY = pos.z;
            if (pos.z > maxY) maxY = pos.z;
        }



        for (int x = 0; x < xgrids; x++)
        {
            for (int y = 0; y < ygrids; y++)
            {
                GameObject obj = new GameObject();
                grids[x, y] = obj;
                obj.name = "Grid_" + x + "_" + y;
                obj.transform.SetParent(transform);
            }
        }

        for (int i = 0; i < meshFilters.Length; i++)
        {
            GameObject obj = meshFilters[i].gameObject;
            if (!isStatic(obj)) continue;

            Vector3 pos = obj.transform.position;

            int xCell = (int) Mathf.Lerp(0, xgrids-1, (pos.x - minX) / (maxX - minX));
            int yCell = (int) Mathf.Lerp(0, ygrids-1, (pos.z - minY) / (maxY - minY));
            obj.transform.SetParent(grids[xCell, yCell].transform, true);
        }

        for (int x = 0; x < xgrids; x++)
        {
            for (int y = 0; y < ygrids; y++)
            {
                
            }
        }
    }

    void StaticMerging(GameObject parent)
    {
        float time = Time.realtimeSinceStartup;
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            GameObject obj = meshFilters[i].gameObject;
            //obj.transform.position = Vector3.zero;
            if (isStatic(obj)){
                CombineInstance combine = new CombineInstance();
                Material mat = obj.GetComponent<Renderer>().sharedMaterial;
                combine.mesh = meshFilters[i].sharedMesh;
                combine.transform = meshFilters[i].transform.localToWorldMatrix;

                SingleBatch c = new SingleBatch();

                int index = MaterialInArray(mat);
                if (MaterialInArray(mat) >= 0)
                    batches[index].combines.Add(combine);
                else
                {
                    c.mat = mat;
                    c.combines.Add(combine);
                    batches.Add(c);
                }

                obj.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        for (int i = 0; i < batches.Count; i++)
        {
            GameObject obj = new GameObject("Static Batch");
            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            meshFilter.mesh = new Mesh();
            meshFilter.mesh.CombineMeshes(batches[i].combines.ToArray());
            meshRenderer.material = batches[i].mat;
            obj.transform.SetParent(transform);
            obj.isStatic = true;
        }

        Debug.Log("Count: " + batches.Count + " Time:" + (Time.realtimeSinceStartup - time));
        batches.Clear();
        batches = null;
        
    }

    int MaterialInArray(Material mat)
    {
        for (int i = 0; i < batches.Count; i++)
        {
            if (batches[i].mat == mat) return i;
        }
        return -1;
    }

    bool isStatic(GameObject obj)
    {
        StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(obj);
        if ((flags & StaticEditorFlags.BatchingStatic) == StaticEditorFlags.BatchingStatic) return true;
        else return false;
    }
}
#endif