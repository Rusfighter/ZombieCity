﻿using Assets.Scripts;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    public GameObject[] weapons;
    public Transform weaponContainer;
    private int weaponIndex = -1;

    private Animator charAnimator;
    private string animatorString = "WeaponType_int";

    private Weapon weapon;

    public Weapon Weapon { get { return weapon; } }
    public int WeaponIndex { get { return weaponIndex; } }


    void Awake()
    {
        charAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void nextWeapon()
    {
        int index = weaponIndex + 1;
        if (index >= weapons.Length) index = 0;
        setWeapon(index);
    }

    public void previousWeapon()
    {
        int index = weaponIndex - 1;
        if (index < 0) index = 0;
        setWeapon(index);
    }

    //TODO create fix that is checking if already exist, maybe precreate all
    public void setWeapon(int index)
    {
        Debug.Log("Set weapon:"+index);
        if (index < 0 || index >= weapons.Length || weaponIndex == index) return;

        weaponIndex = index;

        if (weapon != null)
        {
            weapon.gameObject.SetActive(false);
        }

        Debug.Log(weaponContainer.childCount);

        for (int i = 0; i < weaponContainer.childCount; i++)
        {
            GameObject child = weaponContainer.GetChild(i).gameObject;
            if (child.name == weapons[index].name+"(Clone)")
            {
                setWeapon(child);
                return;
            }
        }

        setWeapon(Instantiate(weapons[index]));
    }

    private void setWeapon(GameObject obj)
    {

        obj.transform.SetParent(weaponContainer, false);
        weapon = obj.GetComponent<Weapon>();
        charAnimator.SetInteger(animatorString, weapon.animationInt);
        obj.SetActive(true);
    }
}
