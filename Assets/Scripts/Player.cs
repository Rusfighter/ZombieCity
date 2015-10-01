using UnityEngine;

namespace Assets.Scripts
{
    public class Player : Humanoid
    {
    
        private Animator charAnimator;
        private WeaponHandler weaponHandler;

        private Transform focus;
        private Enemy focusSrc;

        private Transform focusEffect;

        public Transform Focus
        {
            set {
                if (isDead) return;
                ResetFocus();
                focus = value;
                focusSrc = focus.GetComponent<Enemy>();
                focusSrc.onFocus(this);

                focusEffect.SetParent(focus, false);

                Agent.ResetPath();
            }
            get { return focus; }
        }

        public override void Awake() {
            base.Awake();
            charAnimator = transform.GetChild(0).GetComponent<Animator>();
            weaponHandler = GetComponent<WeaponHandler>();
            focusEffect = transform.FindChild("FocusEffect");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 30;
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
            focusEffect.SetParent(transform, false);
        }


        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Enemy"))
                GetHit(Time.deltaTime * 5);
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

            if (focus != null && Vector3.Distance(transform.position, focus.position) <= weaponHandler.Weapon.range)
            {
                Agent.updateRotation = false;
                Vector3 directionVec = focus.position - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVec), Time.deltaTime * Agent.angularSpeed/60f);
                weaponHandler.Weapon.Activate();
            }
            else
            {
                Agent.updateRotation = true;
                weaponHandler.Weapon.Disable();
            }
        }
    }
}
