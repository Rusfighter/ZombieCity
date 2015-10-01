using Assets.Scripts;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    public GameObject[] weapons;
    public Transform weaponContainer;
    private int weaponIndex = -1;

    private Animator charAnimator;

    private Weapon weapon;

    public Weapon Weapon { get { return weapon; } }
    public int WeaponIndex { get { return weaponIndex; } }


    void Awake()
    {
        charAnimator = transform.GetComponentInChildren<Animator>();
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
    public void setWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length || weaponIndex == index) return;

        weaponIndex = index;

        for (int i = 0; i < weaponContainer.childCount; i++)
        {
            GameObject child = weaponContainer.GetChild(i).gameObject;
            if (child.name == weapons[index].name+"(Clone)"){
                setWeapon(child);
                return;
            }
        }

        setWeapon(Instantiate(weapons[index]));
    }

	public void ReloadWeapon () {
        weapon.Reload();
    }

    private void setWeapon(GameObject obj)
    {
        if (weapon != null) {
            weapon.Disable();
            weapon.gameObject.SetActive(false);
        }

        obj.transform.SetParent(weaponContainer, false);
        obj.SetActive(true);
        weapon = obj.GetComponent<Weapon>();
        weapon.Init(charAnimator);
    }
}
