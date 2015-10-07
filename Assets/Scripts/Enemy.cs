using Assets.Scripts;
using UnityEngine;

public class Enemy : Humanoid
{
    public float StopDistance = 30;
    private ParticleSystem particles;
    private Transform particleTransform;
    private CapsuleCollider capsuleCollider;

    private Player player;
    private Transform playerTransform;
    private Vector3 targetPos;

    private Player focussedBy = null; // player who is focussing this zombie
    private bool isEating = false;

    private Animation anim;

    private FastPathFinding fastPath;


    public Player Target {
        set {
                player = value;
                playerTransform = player.transform;
                isEating = false;
            }
    }

    void OnEnable()
    {
        health = baseHealth;
        anim.Play("Zombie_Walk");
        anim["Zombie_Walk"].speed = 2.5f;

        setItemsOffscreen(false);
    }

    public override void Awake()
    {
        base.Awake();
        particles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (particles != null) particleTransform = particles.gameObject.transform;

        anim = GetComponentInChildren<Animation>();
        fastPath = GetComponent<FastPathFinding>();
        fastPath.speed = Agent.speed;
    }

    public override void onDeath()
    {
        base.onDeath();
        capsuleCollider.enabled = false;
        Agent.enabled = false;
        gameObject.SetActive(false);
        /*transform.DOMove(transform.position - transform.up * 1f, 2f).SetDelay(1.5f).OnComplete(()=> {
            gameObject.SetActive(false);
        });*/
    }

    public void GetHit(int damage, Vector3 directionFrom)
    {
        GetHit(damage);
        if (particles != null)
        {
            particles.Stop();
            particleTransform.forward = -directionFrom;
            particles.Play();
        }
    }

    public void onFocus(Player player)
    {
        focussedBy = player;
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
        if (player == null) return;

        targetPos = playerTransform.position;
        float dinstanceToPlayer = Vector3.Distance(targetPos, transform.position);

        if (dinstanceToPlayer > StopDistance)
        {
            setItemsOffscreen(false);
            fastPath.CalculatePath(playerTransform);
        }
        else setItemsOffscreen(true);

        if (Agent.enabled) // set destination every x frames
            setDestination(targetPos);

        if (!isEating && player.isDead)
        {
            if (dinstanceToPlayer < 1.8f)
            {
                isEating = true;
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
