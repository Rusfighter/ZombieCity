using UnityEngine;
using System.Collections;
public class Weapon : MonoBehaviour
{
    public int damage = 10;
	[Range(1, 100)]
	public int accuracy = 100;
    public float shootTime = 0.1f;
    public int animationInt = 1;
	public int clipSize = 30;
	public float reloadTime = 2;

	public Sprite UiIcon;


	private int ammoInClip; 

    public int AmmoInClip {get { return ammoInClip; } }

    public bool IsReloading {get { return isReloading; } }

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

	private Camera mainCamera;
	private float range;

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
		mainCamera = Camera.main;

		float z = mainCamera.WorldToScreenPoint (emitter.transform.position).z;
		range = Vector3.Distance(mainCamera.ViewportToWorldPoint(new Vector3(0f,0.5f, z)), mainCamera.ViewportToWorldPoint(new Vector3(0.5f,0.5f, z))) ;
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
            if (isReloading) return;
            isReloading = true;
            weaponAnimator.SetBool(reloadString, isReloading);
            timeToNextEvent = reloadTime;
        }

        public virtual void Shoot(){
            if (emitter != null)
            {
                shootRay.origin = emitter.transform.position - emitter.forward.normalized;
                shootRay.direction = emitter.forward.normalized;

                if (Physics.Raycast(shootRay, out shootHit, range, shootAbleMask))
                {
					Vector3 viewportPoint = mainCamera.WorldToViewportPoint(shootHit.point);
					if (viewportPoint.x < 0 || viewportPoint.x > 1
					    || viewportPoint.y < 0 || viewportPoint.y > 1){
						return;
					}

					if (ammoInClip == 0) {
						Reload();
						return;
					}
					ammoInClip--;

					StartEffects(shootHit.point);
					StartCoroutine(StopEffects(0.05f));

					if (shootHit.collider.CompareTag("Enemy"))
                    {
						float chance = Mathf.Lerp(100, accuracy, shootHit.distance/range);
						if (Random.Range(1, 100) <= chance){
							Enemy enemy = shootHit.collider.GetComponent<Enemy>();
							enemy.GetHit(damage, shootRay.direction);
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

        protected virtual IEnumerator StopEffects(float delay) {
            if (emitter == null) yield return null;
			yield return new WaitForSeconds (delay);
            if (gunLine != null) gunLine.enabled = false;
            if (gunParticles != null) gunParticles.Stop();
            if (gunLight != null) gunLight.enabled = false;
        }
    }
