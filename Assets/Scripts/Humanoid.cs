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

        public void GetHit(int damage)
        {
            health = health - damage;
        }
    }
}
