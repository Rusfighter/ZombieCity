using UnityEngine;
using System.Collections;
public class Weapon : MonoBehaviour
{
    public enum State {
        RELOADING, IDLE, WAITING
    }

    public enum Type
    {
        HEAVY, LIGHT, PISTOL, MELEE
    }

    public int damage = 10;
	[Range(1, 100)]
	public int accuracy = 100;
    public float shootTime = 0.1f;
    public int animationInt = 1;
	public int clipSize = 30;
	public float reloadTime = 2;
    public bool m_AutoShoot = true;
    public Type m_WeaponType = Type.HEAVY;

	public Sprite UiIcon;

	private int ammoInClip; 
    public int AmmoInClip {get { return ammoInClip; } }


    //public bool IsReloading {get { return isReloading; } }
    //protected bool isReloading = false;

    private State m_CurrentState = State.IDLE;

    [SerializeField]
	private LineRenderer m_GunLine;
    [SerializeField]
    private ParticleSystem m_Particles;
    [SerializeField]
    private Light m_GunLight;
    [SerializeField]
    private Transform m_Emitter;

    private Ray shootRay;
    private RaycastHit shootHit;

    private int shootAbleMask;

    private Animator weaponAnimator; // use this only for reload or shoot effects
    private string reloadString = "Reload_b";
    private string animatorString = "WeaponType_int";

    private float m_EventTime = 0;

	private Camera mainCamera;
	private float range;



    public void Init(Animator weaponAnimator)
    {
        this.weaponAnimator = weaponAnimator;
        m_CurrentState = State.IDLE;
        m_EventTime = 0;
        weaponAnimator.SetInteger(animatorString, animationInt);
        weaponAnimator.SetBool(reloadString, m_CurrentState == State.RELOADING);
    }

    protected void Awake(){
        //Debug.Log(transform.childCount);
        /*gunLine = GetComponentInChildren<LineRenderer>();
        gunParticles = GetComponentInChildren<ParticleSystem>();
        gunLight = GetComponentInChildren<Light>();*/
        shootAbleMask = LayerMask.GetMask("ShootAble");

        ammoInClip = clipSize;
		mainCamera = Camera.main;

		float z = mainCamera.WorldToScreenPoint (m_Emitter.transform.position).z;
		range = Vector3.Distance(mainCamera.ViewportToWorldPoint(new Vector3(0f,0.5f, z)), mainCamera.ViewportToWorldPoint(new Vector3(0.5f,0.5f, z))) ;
    }

    protected void SetState(State newState, float delay)
    {
        m_CurrentState = newState;
        m_EventTime = delay;
    }

    protected void Update()
    {
        //clamp to min 0
        m_EventTime = Mathf.Max(-1, m_EventTime - Time.deltaTime);

        switch (m_CurrentState)
        {
            case State.IDLE:
                break;
            case State.RELOADING:
                if (m_EventTime <= 0)
                {
                    SetState(State.IDLE, 0.05f);
                    ammoInClip = clipSize;
                    weaponAnimator.SetBool(reloadString, m_CurrentState == State.RELOADING);
                }
                break;
            case State.WAITING:
                if (m_EventTime <= 0)
                {
                    SetState(State.IDLE, 0);
                }
                break;
        }
    }

	protected void Reload()
    {
        if (m_CurrentState == State.RELOADING) return;
        SetState(State.RELOADING, reloadTime);
        weaponAnimator.SetBool(reloadString, m_CurrentState == State.RELOADING);
        Debug.Log("start reloading, eventTime:"+m_EventTime);
    }

    //called once per frame
    public void Shoot(){
        //allowed to shoot
        if (m_CurrentState == State.IDLE)
        {
            if (ammoInClip == 0)
            {
                Reload();
                return;
            }

            shootRay.origin = m_Emitter.transform.position - m_Emitter.forward.normalized;
            shootRay.direction = m_Emitter.forward.normalized;

            ammoInClip--;
            SetState(State.WAITING, shootTime);

            StartEffects(m_Emitter.position, m_Emitter.position+ shootRay.direction*10);
            StartCoroutine(StopEffects(0.02f));

            //only if hit
            if (Physics.Raycast(shootRay, out shootHit, range, shootAbleMask))
            {
				/*Vector3 viewportPoint = mainCamera.WorldToViewportPoint(shootHit.point);
				if (viewportPoint.x < 0 || viewportPoint.x > 1
					|| viewportPoint.y < 0 || viewportPoint.y > 1){
					return;
				}*/

				if (shootHit.collider.CompareTag("Enemy"))
                {
					float chance = Mathf.Lerp(100, accuracy, shootHit.distance/range);
					if (Random.Range(1, 100) <= chance){
						//Enemy enemy = shootHit.collider.GetComponent<Enemy>();
						//enemy.GetHit(damage, shootRay.direction);
					}
                }
            }
        }
    }

    protected virtual void StartEffects(Vector3 startPosition, Vector3 endPosition){
        if (m_Emitter == null) return;
        if (m_GunLine != null)
        {
            m_GunLine.SetPosition(0, startPosition);
            m_GunLine.enabled = true;
            m_GunLine.SetPosition(1, endPosition);
        }
        if (m_Particles != null) m_Particles.Play();
        if (m_GunLight != null) m_GunLight.enabled = true;
    }

    protected virtual IEnumerator StopEffects(float delay) {
        if (m_Emitter == null) yield return null;
		yield return new WaitForSeconds (delay);
        if (m_GunLine != null) m_GunLine.enabled = false;
        if (m_Particles != null) m_Particles.Stop();
        if (m_GunLight != null) m_GunLight.enabled = false;
    }
}
