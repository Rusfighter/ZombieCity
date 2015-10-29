using UnityEngine;

public class FastPathFinding : MonoBehaviour {
    public float speed; // units/s
    private NavMeshPath path;
    private float previousTime;
    Vector3[] corners = new Vector3[30];
    private Transform _transform;

    void Awake()
    {
        path = new NavMeshPath();
        _transform = transform;
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
        int cornersCount = path.GetCornersNonAlloc(corners);
        for (int i = 0; i < cornersCount - 1; i++)
        {
            float distance = Vector3.Distance(corners[i], corners[i + 1]);
            if (distance < distanceToTravel){
                distanceToTravel -= distance;
                continue;
            }
            else {
                _transform.position = Vector3.Lerp(corners[i], corners[i + 1], distanceToTravel / distance);
                break;
            }
        }
    }
}
