using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Humanoid : MonoBehaviour
    {
        protected NavMeshAgent agent;
        public int health;

        public virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        public void setDestination(Vector3 point)
        {
            agent.SetDestination(point);
        }

        public void GetHit(int damage)
        {
            health = health - damage;
        }
    }
}
