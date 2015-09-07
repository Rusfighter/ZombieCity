using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform player;

	private NavMeshAgent nav;
	private Animator charAnimator;

	// Use this for initialization
	void Start () {
		charAnimator = transform.GetChild (0).GetComponent<Animator> ();
		nav = GetComponent <NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		nav.SetDestination (player.position);
		if (nav.velocity.magnitude > 0.5f) {
			charAnimator.SetFloat ("Forward", 0.6f);
		}else
			charAnimator.SetFloat ("Forward", 0);
	}
}
