using System;
using System.Collections;
using UnityEngine;
namespace Assets.Scripts
{
    public class Weapon : MonoBehaviour
    {
        public int damage = 10;
        public float range = 15f;
        public float shootTime = 0.1f;
        public int animationInt = 1;
		public int clipSize = 30;
		public float reloadTime = 2;
		private int ammoInClip; 

        protected bool isAutoShooting = false;
		protected bool isReloading = false;

		private LineRenderer gunLine;
        private ParticleSystem gunParticles;
        private Light gunLight;
        private Transform emitter;

        private Ray shootRay;
        private RaycastHit shootHit;

        private int shootAbleMask;

        public Animator weaponAnimator; // use this only for reload or shoot effects
        private string reloadString = "Reload_b";
        private string animatorString = "WeaponType_int";

        private float timeToNextEvent = 0;

        public void Init(Animator weaponAnimator)
        {
            this.weaponAnimator = weaponAnimator;
            isReloading = false;
            isAutoShooting = false;
            timeToNextEvent = 0;
            weaponAnimator.SetInteger(animatorString, animationInt);
            weaponAnimator.SetBool(reloadString, isReloading);
        }

        public virtual void Awake(){

            emitter = transform.GetChild(0);
            gunLine = GetComponentInChildren<LineRenderer>();
            gunParticles = GetComponentInChildren<ParticleSystem>();
            gunLight = GetComponentInChildren<Light>();
            shootAbleMask = LayerMask.GetMask("ShootAble");

            ammoInClip = clipSize;
        }

        void Update()
        {
            timeToNextEvent -= Time.deltaTime;
            if (timeToNextEvent <= 0)
            {
                if (isReloading)
                {
                    isReloading = false;
                    ammoInClip = clipSize;
                    weaponAnimator.SetBool(reloadString, isReloading);
                }
                else if (isAutoShooting)
                {
                    timeToNextEvent += shootTime;
                    Shoot();
                }
                else timeToNextEvent = 0;
            }
        }

        public void Activate()
        {
            if (isAutoShooting) return;
            isAutoShooting = true;
        }

        public void Disable()
        {
            if (!isAutoShooting) return;
            isAutoShooting = false;
        }

		public virtual void Reload()
        {
            isReloading = true;
            weaponAnimator.SetBool(reloadString, isReloading);
            timeToNextEvent = reloadTime;
            Debug.Log("Reload");
        }

        public virtual void Shoot(){
            if (emitter != null)
            {
                if (ammoInClip == 0) {
                    Reload();
                    return;
                }
                shootRay.origin = emitter.transform.position - emitter.forward;
                shootRay.direction = emitter.forward;
                ammoInClip--;
                if (Physics.Raycast(shootRay, out shootHit, range + emitter.forward.magnitude, shootAbleMask))
                {
                    if (shootHit.collider.CompareTag("Enemy"))
                    {
                        Enemy enemy = shootHit.collider.GetComponent<Enemy>();
                        enemy.GetHit(damage, shootRay.direction);
                        StartEffects(shootHit.point);
                        Invoke("StopEffects", 0.05f);
                    }
                }
            }
        }

        protected virtual void StartEffects(Vector3 position){
            if (emitter == null) return;
            if (gunLine != null && position.magnitude != 0)
            {
                gunLine.SetPosition(0, emitter.transform.position);
                gunLine.enabled = true;
                gunLine.SetPosition(1, position);
            }
            if (gunParticles != null) gunParticles.Play();
            if (gunLight != null) gunLight.enabled = true;
        }

        protected virtual void StopEffects() {
            if (emitter == null) return;
            if (gunLine != null) gunLine.enabled = false;
            if (gunParticles != null) gunParticles.Stop();
            if (gunLight != null) gunLight.enabled = false;
        }
    }
}
