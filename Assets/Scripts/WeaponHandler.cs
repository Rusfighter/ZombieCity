using Assets.Scripts;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {
    public int startWeapon = 0;
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
		//init all weapons
		setWeapon (0);
		setWeapon (1);

		setWeapon(startWeapon);
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
		GameObject weapon = getNextWeapon ();
		if (weapon != null)
			setWeapon (weapon);
		else setWeapon(Instantiate(weapons[index]));

		weaponIndex = index;
    }

	public void ReloadWeapon () {
        weapon.Reload();
    }

	public GameObject getNextWeapon(){
		int index = weaponIndex + 1;
		if (index >= weapons.Length) index = 0;

		for (int i = 0; i < weaponContainer.childCount; i++)
		{
			GameObject child = weaponContainer.GetChild(i).gameObject;
			if (child.name == weapons[index].name+"(Clone)") return child;
		}
		return null;
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
		weapon.Activate ();
    }
}
