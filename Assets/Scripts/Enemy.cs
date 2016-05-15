using UnityEngine;

public class Enemy : Humanoid
{
    public float m_StopDistance = 30;
    private ParticleSystem m_Particles;
    private Transform m_ParticleTrasform;
    private CapsuleCollider m_CapsuleCollider;

    private Player m_Player;
    private Transform m_PlayerTransform;
    private Vector3 m_TargetPosition;

    private bool m_IsEating = false;

    private Animation m_Animation;

    private FastPathFinding m_FastPath;

	private Transform m_ZombieParticleTrasnform;
	private ParticleSystem m_ZombieDeathParticle;


    public Player Target {
        set {
                m_Player = value;
                m_PlayerTransform = m_Player.transform;
                m_IsEating = false;
            }
    }

    void OnEnable()
    {
        m_Health = m_BaseHealth;
        m_Animation.Play("Zombie_Walk");
        m_Animation["Zombie_Walk"].speed = 2.5f;

        setItemsOffscreen(false);
    }

    public override void Awake()
    {
        base.Awake();
        m_Particles = GetComponentInChildren<ParticleSystem>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        if (m_Particles != null) m_ParticleTrasform = m_Particles.gameObject.transform;

        m_Animation = GetComponentInChildren<Animation>();
        m_FastPath = GetComponent<FastPathFinding>();
        m_FastPath.speed = Agent.speed;

		m_ZombieParticleTrasnform = GameObject.Find ("ZombieDeadParticle").transform;
		m_ZombieDeathParticle = m_ZombieParticleTrasnform.GetComponent<ParticleSystem> ();
    }

    public override void onDeath()
    {
        base.onDeath();
        m_CapsuleCollider.enabled = false;
        Agent.enabled = false;
        gameObject.SetActive(false);
        // particle animation
		m_ZombieParticleTrasnform.position = transform.position;
		m_ZombieDeathParticle.Stop ();
		m_ZombieDeathParticle.Play ();
    }

    public void GetHit(int damage, Vector3 directionFrom)
    {
        Damage(damage);
        if (m_Particles != null)
        {
            m_Particles.Stop();
            m_ParticleTrasform.forward = -directionFrom;
            m_Particles.Play();
        }
    }

    void Update()
    {
        int random = Random.Range(1, 3);
        if (!Agent.enabled)
            random = Random.Range(1, 10);

        if (Time.frameCount % random == 0)
            SlowUpdate();
    }

    public void SlowUpdate()
    {
        if (m_Player == null) return;

        m_TargetPosition = m_PlayerTransform.position;
        float dinstanceToPlayer = Vector3.Distance(m_TargetPosition, transform.position);

        if (dinstanceToPlayer > m_StopDistance)
        {
            setItemsOffscreen(false);
            m_FastPath.CalculatePath(m_PlayerTransform);
        }
        else setItemsOffscreen(true);

        if (Agent.enabled) // set destination every x frames
            setDestination(m_TargetPosition);

        if (!m_IsEating && m_Player.isDead)
        {
            if (dinstanceToPlayer < 1.8f)
            {
                m_IsEating = true;
            }
        }
    }

    void setItemsOffscreen(bool active)
    {
        if (Agent.enabled == active) return;

        GetComponent<CapsuleCollider>().enabled = active;
        Agent.enabled = active;

        for (int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
    }
}
