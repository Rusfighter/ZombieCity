

#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;

public class StaticLightMapping : MonoBehaviour {
    public Shader lightMapShader;
    public Shader nonLightMapShader;
    public bool setShader = false;

    public void SetLightMapStatic()
    {
        Light[] lights = FindObjectsOfType<Light>();
        MeshFilter[] meshes = FindObjectsOfType<MeshFilter>();
        for (int i = 0; i<meshes.Length; i++)
        {
            GameObject obj = meshes[i].gameObject;
            StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(obj);
            flags = flags & ~(StaticEditorFlags.LightmapStatic);

            Shader finalShader = nonLightMapShader;

            Vector3[] vertices = meshes[i].sharedMesh.vertices;
            bool found = false;

            for (int y = 0; y<vertices.Length && !found; y++)
            {
                Vector3 vertice = obj.transform.TransformPoint(vertices[y]);
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
                            finalShader = lightMapShader;
                            break;
                        }
                    }else if (light.type == LightType.Spot) {
                        if (InsideCone(lightTransform, vertice, light.range, light.spotAngle))
                        {
                            found = true;
                            flags = flags | StaticEditorFlags.LightmapStatic;
                            finalShader = lightMapShader;
                            break;
                        }
                    }
                }
            }
            if (setShader) obj.GetComponent<Renderer>().sharedMaterial.shader = finalShader;
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