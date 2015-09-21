using UnityEngine;

namespace Assets.Scripts
{
    class Player : Humanoid
    {
    
        private Animator charAnimator;
        private Transform focus;
        private WeaponHandler weaponHandler;

        public int currentWeapon = 1;

        public Transform Focus
        {
            set {
                if (focus != null) focus.localScale /= 1.3f;
                focus = value;
                focus.localScale *= 1.3f;
                agent.ResetPath();
            }
            get { return focus; }
        }

        public override void Awake()
        {
            base.Awake();

            //do stuff
        }


        void Start()
        {
            charAnimator = transform.GetChild(0).GetComponent<Animator>();
            charAnimator.SetInteger("WeaponType_int", currentWeapon+1);
            weaponHandler = GetComponent<WeaponHandler>();
            weaponHandler.setWeapon(0, charAnimator);
        }

        void Update()
        {
            LookAtEnemy();
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            Vector3 lookTo = transform.forward.normalized;
            lookTo.y = 0;
            Vector3 moveDirection = agent.velocity.normalized;
            moveDirection.y = 0;

            float angle = Vector3.Angle(moveDirection, lookTo) * Mathf.Deg2Rad;
            charAnimator.SetFloat("Forward", Mathf.Cos(angle) * agent.velocity.magnitude / agent.speed);


            lookTo = Vector3.Cross(lookTo, Vector3.up);
            angle = Vector3.Angle(moveDirection, lookTo) * Mathf.Deg2Rad;
            charAnimator.SetFloat("Turn", Mathf.Cos(angle) * agent.velocity.magnitude / agent.speed);
        }

        void LookAtEnemy()
        {
            if (focus != null && Vector3.Distance(transform.position, focus.position) <= weaponHandler.currentWeapon.range)
            {
                agent.updateRotation = false;
                Vector3 directionVec = focus.position - transform.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVec), Time.deltaTime * agent.angularSpeed/60f);
                weaponHandler.StartShooting();
            }
            else
            {
                agent.updateRotation = true;
                weaponHandler.StopShooting();
            }
        }
    }
}
