using UnityEngine;

public class FastPathFinding : MonoBehaviour {
    public float speed; // units/s
    private NavMeshPath path;
    private float previousTime;
    void Awake()
    {
        path = new NavMeshPath();
    }
    public void CalculatePath(Transform target)
    {
        Move(path, Time.timeSinceLevelLoad - previousTime);
        previousTime = Time.timeSinceLevelLoad;
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
    }

    void Move(NavMeshPath path, float elapsed)
    {
        if (path == null) return;
        float distanceToTravel = speed * elapsed;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            float distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
            if (distance < distanceToTravel){
                distanceToTravel -= distance;
                continue;
            }
            else {
                Vector3 pos = Vector3.Lerp(path.corners[i], path.corners[i+1], distanceToTravel/distance);
                transform.position = pos;
                break;
            }
        }
    }
}
