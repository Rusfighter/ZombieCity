using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts
{
    public class Player : Humanoid
    {
    
        private Animator charAnimator;
        private WeaponHandler weaponHandler;

        private Transform focus;
        private Enemy focusSrc;

        public Transform Focus
        {
            set {
                if (isDead) return;
                ResetFocus();
                focus = value;
                focusSrc = focus.GetComponent<Enemy>();
                focusSrc.onFocus(this);

                Agent.ResetPath();
            }
            get { return focus; }
        }

        public override void Awake() {
            base.Awake();
            charAnimator = transform.GetChild(0).GetComponent<Animator>();
            weaponHandler = GetComponent<WeaponHandler>();
			Agent.updateRotation = false;


            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            //Application.targetFrameRate = 30;
        }

		void Start(){
			weaponHandler.Weapon.Activate();
		}

        public override void GetHit(float damage) {
            base.GetHit(damage);
        }

        public override void onDeath()
        {
            base.onDeath();
            charAnimator.SetBool("Death_b", isDead);
            ResetFocus();
        }

        public void ResetFocus() {
            focus = null;
            focusSrc = null;
        }


        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
                GetHit(Time.deltaTime * 5);
        }

        void Update() {
            if (isDead) return;
            //LookAtEnemy();
            UpdateAnimation();
		}

        private void UpdateAnimation() {
            Vector3 lookTo = transform.forward.normalized;
            lookTo.y = 0;
            Vector3 moveDirection = Agent.velocity.normalized;
            moveDirection.y = 0;

            float angle = Vector3.Angle(moveDirection, lookTo) * Mathf.Deg2Rad;
            charAnimator.SetFloat("Forward", Mathf.Cos(angle) * Agent.velocity.magnitude / Agent.speed);

            lookTo = Vector3.Cross(lookTo, Vector3.up);
            angle = Vector3.Angle(moveDirection, lookTo) * Mathf.Deg2Rad;
            charAnimator.SetFloat("Turn", Mathf.Cos(angle) * Agent.velocity.magnitude / Agent.speed);
        }

		public void SetForward(Vector3 forward){
			transform.forward = forward.normalized;
		}

		public void SetMovementDirection(Vector3 dir){
			Agent.velocity = dir.normalized * Agent.speed;
		}
    }
}
