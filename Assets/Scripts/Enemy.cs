using Assets.Scripts;
using UnityEngine;
using DG.Tweening;

public class Enemy : Humanoid
{
    public float StopDistance = 30;
    private ParticleSystem particles;
    private Transform particleTransform;
    private Animator charAnimator;
    private CapsuleCollider capsuleCollider;

    private Player player;
    private Transform playerTransform;
    private Vector3 targetPos;

    private Player focussedBy = null; // player who is focussing this zombie
    private bool isEating = false;



    public Player Target {
        set {
                player = value;
                playerTransform = player.transform;
                isEating = false;
                if (charAnimator != null) charAnimator.SetBool("isEating", false);

                setDestination(playerTransform.position);
            }
    }

    void OnEnable()
    {
        if (charAnimator != null) charAnimator.SetBool("isDead", false);
        capsuleCollider.enabled = true;
        Agent.enabled = true;
        health = baseHealth;
    }

    public override void Awake()
    {
        base.Awake();
        particles = GetComponentInChildren<ParticleSystem>();
        charAnimator = GetComponentInChildren<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        if (particles != null) particleTransform = particles.gameObject.transform;
    }

    public override void onDeath()
    {
        base.onDeath();
        if (charAnimator != null) charAnimator.SetBool("isDead", true);
        capsuleCollider.enabled = false;
        Agent.enabled = false;
        transform.DOMove(transform.position - transform.up * 1f, 2f).SetDelay(1.5f).OnComplete(()=> {
            gameObject.SetActive(false);
        });
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

    void SlowUpdate()
    {
        if (player == null) return;

        targetPos = playerTransform.position;
        float dinstanceToPlayer = Vector3.Distance(targetPos, transform.position);

        if (dinstanceToPlayer > StopDistance)
        {
            Agent.enabled = false;
            charAnimator.enabled = false;
        }
        else if (!Agent.enabled && !charAnimator.enabled)
        {
            Agent.enabled = true;
            charAnimator.enabled = true;
        }

        if (Agent.enabled) // set destination every x frames
            setDestination(targetPos);

        if (!isEating && player.isDead)
        {
            if (dinstanceToPlayer < 1.8f)
            {
                isEating = true;
                if (charAnimator != null) charAnimator.SetBool("isEating", true);
            }
        }
    }

    void Update()
    {
        if (Time.frameCount % 3 == 0)
            SlowUpdate();
    }
}
