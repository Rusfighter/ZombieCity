using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class Humanoid : MonoBehaviour
    {
        protected NavMeshAgent agent;
        public int health;

        public void GetHit(int damage)
        {
            health = health - damage;
        }
    }
}
