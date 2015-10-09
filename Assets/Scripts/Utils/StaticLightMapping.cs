

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class StaticLightMapping : MonoBehaviour {
    public Shader finalShader;
    public LayerMask nonLightMap;

    public void SetLightMapStatic()
    {
        Light[] lights = FindObjectsOfType<Light>();
        MeshFilter[] meshes = FindObjectsOfType<MeshFilter>();
        for (int i = 0; i<meshes.Length; i++)
        {
            GameObject obj = meshes[i].gameObject;
            StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(obj);
            flags = flags & ~(StaticEditorFlags.LightmapStatic);

            Vector3[] vertices = meshes[i].sharedMesh.vertices;
            bool found = false;

            for (int y = 0; y<vertices.Length-2 && !found; y++)
            {
                Vector3 vertice1 = vertices[y];
                Vector3 vertice2 = vertices[y+1];
                Vector3 vertice3 = vertices[y+2];
                for (int v = 0; v <= 6; v++)
                {
                    Vector3 vertice;
                    if (v <= 3) vertice = vertice1 + (vertice2 - vertice1) * v/3f;
                    else vertice = vertice1 + (vertice3 - vertice1) * (v-3) / 3f;

                    vertice = obj.transform.TransformPoint(vertice);
                    for (int z = 0; z < lights.Length; z++)
                    {
                        Light light = lights[z];
                        if (!light.enabled) continue;
                        Transform lightTransform = light.transform;

                        if (light.type == LightType.Point)
                        {
                            if (Vector3.Distance(lightTransform.position, vertice) < light.range)
                            {
                                found = true;
                                flags = flags | StaticEditorFlags.LightmapStatic;
                                break;
                            }
                        }
                        else if (light.type == LightType.Spot)
                        {
                            if (InsideCone(lightTransform, vertice, light.range, light.spotAngle))
                            {
                                found = true;
                                flags = flags | StaticEditorFlags.LightmapStatic;
                                break;
                            }
                        }
                    }
                }
            }

            if (finalShader != null) obj.GetComponent<Renderer>().sharedMaterial.shader = finalShader;
            GameObjectUtility.SetStaticEditorFlags(obj, flags);

        }
    }

    bool InsideCone(Transform coneTransform, Vector3 point, float height, float angle)
    {
        float halfangle = angle/2f;
        Vector3 X = point;
        Vector3 M = coneTransform.position;
        float angleBetween = Vector3.Angle(coneTransform.forward.normalized, (X-M).normalized);
        return angleBetween <= halfangle
            && Vector3.Magnitude(M-X) <= height/Mathf.Cos(angleBetween * Mathf.Deg2Rad);

  }
}
#endif