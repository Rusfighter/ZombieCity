using Assets.Scripts;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    public Weapon[] weapons;
    public Transform weaponContainer; // hand

    public int currentWeaponIdx { get; set; }

    public void setWeapon(int index, Animator anim, string variable){
        if (index < 0 || index >= weapons.Length) return;

        for (int i = 0; i < weaponContainer.childCount; i++)
        {
            Destroy(weaponContainer.GetChild(i).gameObject);
        }
        GameObject obj = Instantiate(weapons[index].obj);
        obj.transform.SetParent(weaponContainer, false);

        anim.SetInteger(variable, weapons[index].animation);

        currentWeaponIdx = index;
    }
}
