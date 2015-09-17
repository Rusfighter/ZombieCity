using System;
using Assets.Scripts;
using UnityEngine;

public class Enemy : Humanoid
{
    public Transform player;
    private ParticleSystem particles;
    private Transform particleTransform;
    //private Animator charAnimator;

    public override void Awake()
    {
        base.Awake();

        particles = GetComponentInChildren<ParticleSystem>();
        if (particles != null) particleTransform = particles.gameObject.transform;
    }

    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        //charAnimator = GetComponent<Animator>();
    }

    public void GetHit(int damage, Vector3 directionFrom)
    {
        GetHit(damage);
        if (health < 1) health = 200;

        if (particles != null)
        {
            particles.Stop();
            particleTransform.forward = -directionFrom;
            particles.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }
}
