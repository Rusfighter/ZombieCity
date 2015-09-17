using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    public Weapon[] weapons;
    public Transform weaponContainer; // hand
    public int currentWeaponIdx { get; set; }

    private LineRenderer gunLine;
    private ParticleSystem gunParticles;
    private Light gunLight;
    private Transform emitter;

    private bool shooting = false;

    public void StartShooting()
    {
        if (shooting) return;
        shooting = true;
        StartCoroutine(Shoot());
    }

    public void StopShooting()
    {
        if (!shooting) return;
        shooting = false;
        StopCoroutine(Shoot());
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

    private IEnumerator Shoot()
    {
        while (true)
        {

            //calculate end line and see if it hits someone
            if (emitter != null)
            {
                Vector3 lineEnd = emitter.transform.position + emitter.forward * weapons[currentWeaponIdx].range;
                StartEffects(lineEnd);
            }

            Invoke("StopEffects", 0.05f);

            yield return new WaitForSeconds(weapons[currentWeaponIdx].shootTime);
        }
    }


    public void setWeapon(int index, Animator anim, string variable = "WeaponType_int")
    {
        if (index < 0 || index >= weapons.Length) return;

        StopShooting();

        for (int i = 0; i < weaponContainer.childCount; i++)
        {
            Destroy(weaponContainer.GetChild(i).gameObject);
        }
        GameObject obj = Instantiate(weapons[index].obj);
        obj.transform.SetParent(weaponContainer, false);

        anim.SetInteger(variable, weapons[index].animation);
        currentWeaponIdx = index;

        if (obj.transform.childCount > 0)
        {
            emitter = obj.transform.GetChild(0);
            gunLine = obj.GetComponentInChildren<LineRenderer>();
            gunParticles = obj.GetComponentInChildren<ParticleSystem>();
            gunLight = obj.GetComponentInChildren<Light>();
        }
        else { emitter = null; }


        StartShooting();
    }
}
