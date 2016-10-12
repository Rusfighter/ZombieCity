using UnityEngine;

public class FastPathFinding {
    //public float speed; // units/s
    private NavMeshPath path;
    private float previousTime;
    Vector3[] corners;
    private Transform _transform;

    public FastPathFinding(Transform transform, int cornersSize = 30)
    {
        path = new NavMeshPath();
        corners = new Vector3[cornersSize];
        _transform = transform;
    }
    public void MoveTarget(Transform target, float speed)
    {
        Move(path, Time.timeSinceLevelLoad - previousTime, speed);
        previousTime = Time.timeSinceLevelLoad;
        NavMesh.CalculatePath(_transform.position, target.position, NavMesh.AllAreas, path);
    }

    void Move(NavMeshPath path, float elapsed, float speed)
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
