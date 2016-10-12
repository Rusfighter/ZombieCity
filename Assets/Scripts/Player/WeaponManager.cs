using UnityEngine;

public class WeaponManager : MonoBehaviour {
    public int m_startWeapon = 0;
    public Weapon[] m_Weapons;
    public Transform m_WeaponContainer;
    public Animator m_CharAnimator;
    private int m_WeaponIndex = -1;


    private Weapon m_Weapon;

    public Weapon Weapon { get { return m_Weapon; } }


    void Awake()
    {
		setWeapon(m_startWeapon);
    }

    public void nextWeapon()
    {
        int index = m_WeaponIndex + 1;
        if (index >= m_Weapons.Length) index = 0;
        setWeapon(index);
    }

    public void previousWeapon()
    {
        int index = m_WeaponIndex - 1;
        if (index < 0) index = 0;
        setWeapon(index);
    }


    public void setWeapon(int index)
    {
        if (index < 0 || index >= m_Weapons.Length || m_WeaponIndex == index) return;
        m_WeaponIndex = index;

        //disable everything
        int size = m_WeaponContainer.childCount;
        for (int i = 0; i< size;i++)
        {
            m_WeaponContainer.GetChild(i).gameObject.SetActive(false);
        }

        //find if object exists already
        for (int i = 0; i < size; i++)
        {
            GameObject child = m_WeaponContainer.GetChild(i).gameObject;
            if (child.name == m_Weapons[index].gameObject.name + "(Clone)") {
                child.SetActive(true);
                m_Weapon = child.GetComponent<Weapon>();
                m_Weapon.Init(m_CharAnimator);
                return;
            }
        }

        //create new 
        //m_Weapon = m_Weapons[index];
        GameObject obj = Instantiate(m_Weapons[index].gameObject);
        obj.transform.SetParent(m_WeaponContainer, false);
        obj.SetActive(true);
        m_Weapon = obj.GetComponent<Weapon>();
        m_Weapon.Init(m_CharAnimator);

        Debug.Log("set weapon");
        
    }
}
