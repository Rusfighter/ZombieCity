using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    class Player : Humanoid
    {
    
        private Animator charAnimator;
        private Transform focus;
        private WeaponHandler weaponHandler;

        public int currentWeapon = 1;

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
            if (Input.GetMouseButtonDown(1))
            {
                ClickAction(Input.mousePosition);

            }
            else if (Input.touchCount > 0)
            {
                ClickAction(Input.GetTouch(0).position);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                weaponHandler.setWeapon(weaponHandler.currentWeaponIdx + 1, charAnimator);
            }

            if (focus != null)
            {
                Vector3 delta = -transform.position + focus.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(delta), 20 * Time.deltaTime);
            }
            else
                agent.updateRotation = true;

            //normalize vectors
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

        void ClickAction(Vector3 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Enemy")
                {
                    Debug.Log("Enemy");
                    focus = hit.collider.transform;
                    agent.ResetPath();
                    agent.updateRotation = false;
                }
                else
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
