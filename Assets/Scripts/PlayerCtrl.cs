using UnityEngine;

[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (CapsuleCollider))]

public class PlayerCtrl : MonoBehaviour {
	private Animator charAnimator;
	private Animator weaponsAnimator;
	private NavMeshAgent nav;   

	private Transform focus;
	// Use this for initialization
	void Start () {
		charAnimator = transform.GetChild (0).GetComponent<Animator> ();
		weaponsAnimator = transform.GetChild (1).GetComponent<Animator> ();
		nav = GetComponent <NavMeshAgent> ();
		charAnimator.SetInteger ("WeaponType_int", 2);
		weaponsAnimator.SetInteger ("WeaponType_int", 2);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (1)) {
			ClickAction(Input.mousePosition);

		} else if (Input.touchCount > 0) {
			ClickAction(Input.GetTouch (0).position);
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			charAnimator.SetBool("Shoot_b", true);
			weaponsAnimator.SetBool("Shoot_b", true);
		}

		if (focus != null) {
			Vector3 delta = -transform.position + focus.position;
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (delta), 20 * Time.deltaTime);
		} else
			nav.updateRotation = true;


		//
		//normalize vectors
		Vector3 lookTo = transform.forward.normalized;
		lookTo.y = 0;
//		Debug.Log (lookTo);
		Vector3 moveDirection = nav.velocity.normalized;
		moveDirection.y = 0;

		float angle = Vector3.Angle (moveDirection, lookTo) * Mathf.Deg2Rad;
		charAnimator.SetFloat ("Forward", Mathf.Cos (angle) * nav.velocity.magnitude / nav.speed);


		lookTo = Vector3.Cross (lookTo, Vector3.up);
		angle = Vector3.Angle (moveDirection, lookTo) * Mathf.Deg2Rad;
		charAnimator.SetFloat ("Turn", Mathf.Cos (angle) * nav.velocity.magnitude / nav.speed);

	}

	void ClickAction(Vector3 position){
		Ray ray = Camera.main.ScreenPointToRay (position);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100)) {
			if (hit.collider.tag == "Enemy"){
				Debug.Log ("Enemy");
				focus = hit.collider.transform;
				nav.ResetPath();
				nav.updateRotation = false;
			}else{
				nav.SetDestination (hit.point);
				//focus = null;
			}
		}
	}
}
