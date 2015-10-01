using UnityEngine;

//==============================================================================
public class MeshMerger : MonoBehaviour
{
    public MeshFilter[] meshFilters;
    MeshFilter meshFilter;
    Mesh finalMesh;
    public Material material;

    //----------------------------------------------------------------------------
    void Start()
    {
        // if not specified, go find meshes
        if (meshFilters.Length == 0)
        {
            // find all the mesh filters
            Component[] comps = GetComponentsInChildren(typeof(MeshFilter));
            meshFilters = new MeshFilter[comps.Length];

            int mfi = 0;
            foreach (Component comp in comps)
                meshFilters[mfi++] = (MeshFilter)comp;
        }

        // figure out array sizes
        int vertCount = 0;
        int normCount = 0;
        int triCount = 0;
        int uvCount = 0;

        foreach (MeshFilter mf in meshFilters)
        {
            vertCount += mf.mesh.vertices.Length;
            normCount += mf.mesh.normals.Length;
            triCount += mf.mesh.triangles.Length;
            uvCount += mf.mesh.uv.Length;
            if (material == null)
                material = mf.gameObject.GetComponent<Renderer>().material;
        }

        // allocate arrays
        Vector3[] verts = new Vector3[vertCount];
        Vector3[] norms = new Vector3[normCount];
        int[] tris = new int[triCount];
        Vector2[] uvs = new Vector2[uvCount];
        int vertOffset = 0;
        int normOffset = 0;
        int triOffset = 0;
        int uvOffset = 0;

        meshFilter = gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        foreach (MeshFilter mf in meshFilters)
        {
            Mesh m = mf.mesh;

            foreach (int i in m.triangles)
            {
                tris[triOffset++] = i + vertOffset;
            }

            foreach (Vector3 lv in m.vertices)
            {
                Vector3 vert = mf.transform.TransformPoint(lv);
                verts[vertOffset++] = meshFilter.transform.InverseTransformPoint(vert);
            }

            foreach (Vector3 lv in m.normals)
            {
                Vector3 normal = mf.transform.TransformDirection(lv);
                norms[normOffset++] = meshFilter.transform.InverseTransformPoint(normal);
            }

            foreach (Vector3 v in m.uv)
            {
                uvs[uvOffset++] = new Vector2(v.x, v.y);
            }

            MeshRenderer mr = mf.gameObject.GetComponent<MeshRenderer>();
            if (mr) mr.enabled = false;
        }

        finalMesh = new Mesh();
        finalMesh.name = gameObject.name;
        finalMesh.vertices = verts;
        finalMesh.normals = norms;
        finalMesh.uv = uvs;
        finalMesh.triangles = tris;

        meshFilter.mesh = finalMesh;

        GetComponent<Renderer>().material = material;
    }

    void Update()
    {
        SetVerticesAndNormals();
    }

    void SetVerticesAndNormals()
    {

        Vector3[] verts = finalMesh.vertices;
        Vector3[] norms = finalMesh.normals;
        int[] tris = finalMesh.triangles;
        int vertOffset = 0;
        int normOffset = 0;
        int triOffset = 0;

        foreach (MeshFilter mf in meshFilters)
        {
            Mesh m = mf.mesh;

            foreach (int i in m.triangles)
            {
                tris[triOffset++] = i + vertOffset;
            }

            foreach (Vector3 lv in m.vertices)
            {
                Vector3 vert = mf.transform.TransformPoint(lv);
                verts[vertOffset++] = meshFilter.transform.InverseTransformPoint(vert);
            }

            foreach (Vector3 lv in m.normals)
            {
                Vector3 normal = mf.transform.TransformDirection(lv);
                norms[normOffset++] = meshFilter.transform.InverseTransformDirection(normal);
            }
        }

        finalMesh.vertices = verts;
        finalMesh.normals = norms;
    }
}
