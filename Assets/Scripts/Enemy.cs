using Assets.Scripts;
using UnityEngine;
using DG.Tweening;

public class Enemy : Humanoid
{
    public int DamagePersecond = 1;
    private ParticleSystem particles;
    private Transform particleTransform;
    private Animator charAnimator;
    private CapsuleCollider capsuleCollider;

    private Player player;
    private Transform playerTransform;

    private Player focussedBy = null; // player who is focussing this zombie
    private bool isEating = false;

    public Player Player {
        set {
                player = value;
                playerTransform = player.transform;
                isEating = false;
                charAnimator.SetBool("isEating", false);
            }
    }

    void OnEnable()
    {
        charAnimator.SetBool("isDead", false);
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
        charAnimator.SetBool("isDead", true);
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

    void Update()
    {
        if (player == null) return;

        setDestination(playerTransform.position);

        if (Vector3.Distance(playerTransform.position, transform.position) < 2f && player.isDead && !isEating)
        {
            isEating = true;
            charAnimator.SetBool("isEating", true);
        }
    }
}
