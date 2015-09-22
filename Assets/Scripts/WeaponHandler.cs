using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    public Weapon[] weapons;
    public Transform weaponContainer; // hand
    public int currentWeaponIdx { get; set; }
    public Weapon currentWeapon { get; set; }

    private LineRenderer gunLine;
    private ParticleSystem gunParticles;
    private Light gunLight;
    private Transform emitter;

    private Ray shootRay;
    private RaycastHit shootHit;

    private bool shooting = false;

    private IEnumerator shootCoroutine;

    void Start()
    {
        shootCoroutine = Shoot();
    }

    public void StartShooting()
    {
        if (shooting) return;
        shooting = true;
        StartCoroutine(shootCoroutine);
        //Debug.Log("Start Shooting");
    }

    public void StopShooting()
    {
        if (!shooting) return;
        shooting = false;
        StopCoroutine(shootCoroutine);
        StopEffects();
    }

    private void StartEffects(Vector3 position = new Vector3())
    {
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
    private void StopEffects()
    {
        if (emitter == null) return;
        if (gunLine != null) gunLine.enabled = false;
        if (gunParticles != null) gunParticles.Stop();
        if (gunLight != null) gunLight.enabled = false;
    }

    public void SingleShot()
    {
        if (emitter != null)
        {
            shootRay.origin = emitter.transform.position;
            shootRay.direction = emitter.forward;

            if (Physics.Raycast(shootRay, out shootHit, currentWeapon.range))
            {
                Enemy enemy = shootHit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.GetHit(currentWeapon.damage, shootRay.direction);
                    StartEffects(shootHit.point);
                    Invoke("StopEffects", 0.05f);
                }

            }
        }
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            SingleShot();
            yield return new WaitForSeconds(currentWeapon.shootTime);
        }
    }



    public void setWeapon(int index, Animator anim, string variable = "WeaponType_int")
    {
        if (index < 0 || index >= weapons.Length) return;

        for (int i = 0; i < weaponContainer.childCount; i++)
        {
            Destroy(weaponContainer.GetChild(i).gameObject);
        }
        GameObject obj = Instantiate(weapons[index].obj);
        obj.transform.SetParent(weaponContainer, false);

        anim.SetInteger(variable, weapons[index].animation);
        currentWeaponIdx = index;
        currentWeapon = weapons[index];

        if (obj.transform.childCount > 0)
        {
            emitter = obj.transform.GetChild(0);
            gunLine = obj.GetComponentInChildren<LineRenderer>();
            gunParticles = obj.GetComponentInChildren<ParticleSystem>();
            gunLight = obj.GetComponentInChildren<Light>();
        }
        else { emitter = null; }
    }
}
