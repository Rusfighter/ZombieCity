using UnityEngine;

namespace Assets.Scripts
{
    public class Player : Humanoid
    {
    
        private Animator charAnimator;
        private WeaponHandler weaponHandler;

        private int currentWeapon = 1;

        private Transform focus;
        private Enemy focusSrc;

        public Transform Focus
        {
            set {
                if (isDead) return;
                focus = value;
                focusSrc = focus.GetComponent<Enemy>();
                if (focusSrc != null) focusSrc.onFocus(this);
                Agent.ResetPath();
            }
            get { return focus; }
        }

        public override void Awake() {
            base.Awake();
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
            if (other.tag == "Enemy")
                GetHit(Time.deltaTime * 5);
        }


        void Start() {
            charAnimator = transform.GetChild(0).GetComponent<Animator>();
            charAnimator.SetInteger("WeaponType_int", currentWeapon+1);
            weaponHandler = GetComponent<WeaponHandler>();
            weaponHandler.setWeapon(0, charAnimator);
        }

        void Update() {
            if (isDead) return;
            LookAtEnemy();
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

        void LookAtEnemy() {

            if (focusSrc != null && focusSrc.isDead) ResetFocus();

            if (focus != null && Vector3.Distance(transform.position, focus.position) <= weaponHandler.currentWeapon.range)
            {
                Agent.updateRotation = false;
                Vector3 directionVec = focus.position - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVec), Time.deltaTime * Agent.angularSpeed/60f);
                weaponHandler.StartShooting();
            }
            else
            {
                Agent.updateRotation = true;
                weaponHandler.StopShooting();
            }
        }
    }
}
