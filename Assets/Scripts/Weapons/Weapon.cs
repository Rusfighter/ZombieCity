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
		public int reloadTime = 2;
		private int ammoInClip; 

        protected bool isAutoShooting = false;
		protected bool isReloading = false;

		private LineRenderer gunLine;
        private ParticleSystem gunParticles;
        private Light gunLight;
        private Transform emitter;

		public WeaponHandler weaponHandler;

        private Ray shootRay;
        private RaycastHit shootHit;

        private IEnumerator shootCoroutine;
		private IEnumerator reloadCoroutine;

        private int shootAbleMask;

        public virtual void Awake(){

            emitter = transform.GetChild(0);
            gunLine = GetComponentInChildren<LineRenderer>();
            gunParticles = GetComponentInChildren<ParticleSystem>();
            gunLight = GetComponentInChildren<Light>();
            shootAbleMask = LayerMask.GetMask("ShootAble");

			reloadCoroutine = ReloadRoutine();
            shootCoroutine = ShootRoutine();

			ammoInClip = clipSize;
        }
		private IEnumerator ReloadRoutine() {
			yield return new WaitForSeconds (reloadTime);
			isReloading = false;
			weaponHandler.reload (false);
			ammoInClip = clipSize;
		}

        private IEnumerator ShootRoutine()
        {
            while (true){
				if (!isReloading){
                	Shoot();
				}
                yield return new WaitForSeconds(shootTime);
            }
        }

        public void StartAutoShoot() {
            if (!isAutoShooting)
            {
                isAutoShooting = true;
                StartCoroutine(shootCoroutine);
            }
        }

        public virtual void StopAutoShoot(){
            if (isAutoShooting)
            {
                isAutoShooting = false;
                StopCoroutine(shootCoroutine);
            }
        }
		public void Reload() {
			isReloading = true;
			weaponHandler.reload (true);
			StartCoroutine (ReloadRoutine());
		}

        void Shoot(){

            if (emitter != null) {
				Debug.Log (ammoInClip);
				if (ammoInClip == 0) {
					Reload();
				} else {
					shootRay.origin = emitter.transform.position - emitter.forward;
					shootRay.direction = emitter.forward;
					ammoInClip--;
					if (Physics.Raycast (shootRay, out shootHit, range + emitter.forward.magnitude, shootAbleMask)) {
						if (shootHit.collider.CompareTag ("Enemy")) {
							Enemy enemy = shootHit.collider.GetComponent<Enemy> ();
							enemy.GetHit (damage, shootRay.direction);
							StartEffects (shootHit.point);
							Invoke ("StopEffects", 0.05f);
						}
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
