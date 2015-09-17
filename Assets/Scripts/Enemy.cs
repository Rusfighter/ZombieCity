using System;
using Assets.Scripts;
using UnityEngine;

public class Enemy : Humanoid
{
    public Transform player;
    //private Animator charAnimator;

    public override void Awake()
    {
        base.Awake();

        //do stuff
    }

    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        //charAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }
}
