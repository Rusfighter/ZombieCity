using System;
using Assets.Scripts;
using UnityEngine;

public class Enemy : Humanoid
{
    public Transform player;
    //private Animator charAnimator;

    void Start()
    {
        //charAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
        /*if (nav.velocity.magnitude > 0.5f) {
			charAnimator.SetFloat ("Forward", 0.6f);
		}else
			charAnimator.SetFloat ("Forward", 0);*/
    }
}
