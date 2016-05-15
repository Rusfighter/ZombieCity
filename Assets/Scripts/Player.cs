using UnityEngine;

public class Player : Humanoid
{
    private Animator m_CharAnimator;
    private WeaponHandler m_WeaponHandler;

    public override void Awake() {
        base.Awake();
        m_CharAnimator = transform.GetChild(0).GetComponent<Animator>();
        m_WeaponHandler = GetComponent<WeaponHandler>();
		Agent.updateRotation = false;
        //Application.targetFrameRate = 30;
    }

	void Start(){
		m_WeaponHandler.Weapon.Activate();
	}

    public override void onDeath()
    {
        base.onDeath();
        m_CharAnimator.SetBool("Death_b", isDead);
    }


    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
            Damage(Time.deltaTime * 5);
    }

    void Update() {
        if (isDead) return;
        UpdateAnimation();
	}

    private void UpdateAnimation() {
        Vector3 lookTo = transform.forward.normalized;
        lookTo.y = 0;
        Vector3 moveDirection = Agent.velocity.normalized;
        moveDirection.y = 0;

        float angle = Vector3.Angle(moveDirection, lookTo) * Mathf.Deg2Rad;
        m_CharAnimator.SetFloat("Forward", Mathf.Cos(angle) * Agent.velocity.magnitude / Agent.speed);

        lookTo = Vector3.Cross(lookTo, Vector3.up);
        angle = Vector3.Angle(moveDirection, lookTo) * Mathf.Deg2Rad;
        m_CharAnimator.SetFloat("Turn", Mathf.Cos(angle) * Agent.velocity.magnitude / Agent.speed);
    }

	public void SetForward(Vector3 forward){
		transform.forward = forward.normalized;
	}

	public void SetMovementDirection(Vector3 dir){
		Agent.velocity = dir.normalized * Agent.speed;
	}
}
