using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Humanoid : MonoBehaviour, IDamageable<int>
{
    public float m_MoveSpeed = 2;
    public int m_BaseHealth;
    protected int m_Health;

    public delegate void OnHealthChange(int newHealth);
    public event OnHealthChange onHealthChanged;

    public bool isDead {
        get { return m_Health <= 0; }
    }

    abstract protected void OnAwakeAction();
    abstract protected void OnStartAction();
    abstract protected void OnDeathAction();
    abstract protected void OnDamageAction(float damage);


    private void Awake()
    {
        m_Health = m_BaseHealth;
        OnAwakeAction();
    }

    public virtual void Move(Vector2 dir)
    {
        dir *= m_MoveSpeed * Time.deltaTime;
        transform.position += new Vector3(dir.x , 0, dir.y);
    }

    public virtual void Rotate(Vector3 forward)
    {
        transform.forward = forward.normalized;
    }

    private void Start()
    {
        OnStartAction();
    }

    private void onDeath()
    {
        m_Health = 0;
        OnDeathAction();
    }

    public void Damage(int damageTaken)
    {
        if (isDead) return;
        m_Health = m_Health - damageTaken;
        OnDamageAction(damageTaken);
        if (isDead) onDeath();

        if (onHealthChanged != null)
        {
            onHealthChanged(m_Health);
        }
    }

    public float Health {
		get { return m_Health; }
	}
}
