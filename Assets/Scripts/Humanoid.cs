using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Humanoid : MonoBehaviour
    {
        private NavMeshAgent agent;
        public float baseHealth;
        protected float health;

        public NavMeshAgent Agent {
            get { return agent; } }

        public bool isDead {
            get { return health <= 0; }
        }

        public virtual void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            health = baseHealth;
        }

        public virtual void GetHit(float damage)
        {
            if (isDead) return;
            health = health - damage;
            if (isDead) onDeath();
        }

        public virtual void onDeath()
        {
            agent.ResetPath();
        }

        public void setDestination(Vector3 position) {
            if (isDead) return;
            agent.SetDestination(position);
        }
		public float Health {
			get { return health; }
		}
    }
}
