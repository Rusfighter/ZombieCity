using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider))]


public class Zombie : Humanoid
{
    public enum State
    {
        EATING, CHASING, IDLE
    }

    public float m_StopDistance = 30;
    [SerializeField]
    private ParticleSystem m_Particles;
    private Transform m_ParticleTrasform;
    private CapsuleCollider m_CapsuleCollider;

    private Humanoid m_Target;
    private Transform m_TargetTransform;
    private Vector3 m_TargetPosition;

    [SerializeField]
    private Animation m_Animation;

    [SerializeField]
    private Transform m_ZombieParticleTransform;
	private ParticleSystem m_ZombieDeathParticle;

    protected NavMeshAgent m_Agent;

    private FastPathFinding m_PathFinding;

    private State m_CurrentState = State.IDLE;


    public Humanoid Target {
        set
        {
            m_Target = value;
            if (m_Target != null){
                m_TargetTransform = m_Target.transform;
            }else{
                m_TargetTransform = null;
            }
        }
    }

    void OnEnable()
    {
        m_Health = m_BaseHealth;
        m_Animation.Play("Zombie_Walk");
        m_Animation["Zombie_Walk"].speed = m_MoveSpeed;
    }

    protected override void OnAwakeAction()
    {
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        if (m_Particles != null) m_ParticleTrasform = m_Particles.gameObject.transform;

        m_Animation = GetComponentInChildren<Animation>();
        
		m_ZombieDeathParticle = m_ZombieParticleTransform.GetComponent<ParticleSystem> ();
        m_PathFinding = new FastPathFinding(transform);
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.speed = m_MoveSpeed;

    }

    protected override void OnDeathAction()
    {
        m_CapsuleCollider.enabled = false;
        m_Agent.enabled = false;
        gameObject.SetActive(false);
        // particle animation
		m_ZombieParticleTransform.position = transform.position;
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

    public void Move()
    {

        if (m_Agent.enabled)
            m_Agent.destination = m_TargetTransform.position;
        else
            m_PathFinding.MoveTarget(m_TargetTransform, m_Agent.speed);
    }

    void OnBecameVisible()
    {
        setItemsOffscreen(true);
    }

    void OnBecameInvisible()
    {
        setItemsOffscreen(false);
    }

    void UpdateStates()
    {
        switch (m_CurrentState)
        {
            case State.IDLE:
                if (m_Target != null)
                {
                    m_CurrentState = State.CHASING;
                }
                break;
            case State.CHASING:

                if (m_Target == null){
                    m_CurrentState = State.IDLE;
                    break;
                }
                else
                {
                    Move();

                    //check if in range
                    float dinstanceToPlayer = Vector3.Distance(m_TargetPosition, transform.position);
                    if (m_Target.isDead && dinstanceToPlayer < 1.8f)
                        m_CurrentState = State.EATING;
                }
                

                break;
            case State.EATING:
                Animator anim = GetComponent<Animator>();
                break;
        }
    }

    void Update()
    {
        UpdateStates();
    }

    void setItemsOffscreen(bool active)
    {
        if (m_Agent.enabled == active) return;

        m_CapsuleCollider.enabled = active;
        m_Agent.enabled = active;

        for (int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
    }

    protected override void OnStartAction()
    {

    }

    protected override void OnDamageAction(float damage)
    {

    }
}
