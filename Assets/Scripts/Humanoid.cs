using System;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Humanoid : MonoBehaviour, IDamageable<float>
{
    public float m_BaseHealth;

    protected float m_Health = 100;

    private NavMeshAgent m_Agent;

    public NavMeshAgent Agent {
        get { return m_Agent; } }

    public bool isDead {
        get { return m_Health <= 0; }
    }

    public virtual void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Health = m_BaseHealth;
    }

    public virtual void onDeath()
    {
        m_Agent.ResetPath();
    }

    public void setDestination(Vector3 position) {
        if (isDead) return;
        m_Agent.SetDestination(position);
    }

    public void Damage(float damageTaken)
    {
        if (isDead) return;
        m_Health = m_Health - damageTaken;
        if (isDead) onDeath();
    }

    public float Health {
		get { return m_Health; }
	}
}
