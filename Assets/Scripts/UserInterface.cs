using UnityEngine;
using UnityEngine.UI;


public class UserInterface : MonoBehaviour
{
    [SerializeField]
    private Text m_HealthText;
    [SerializeField]
    private Text m_AmmoText;
    [SerializeField]
    private Text m_WaveText;
    [SerializeField]
    private Text m_ZombiesLeftText;
    [SerializeField]
    private Text m_ClipSizeText;
    [SerializeField]
    private Image m_WeaponImage;
    [SerializeField]
    private Image m_SecondWeaponImage;
    [SerializeField]
    private Text m_AnnouncementText;
    [SerializeField]
    public Slider m_ClipSlider;
   
    [SerializeField]
    private Humanoid m_Humanoid;

    private GameHandler.LevelState state = GameHandler.LevelState.EMPTY;

    public Humanoid Humanoid
    {
        set {
            //unsubscribe
            if (m_Humanoid != null)
            {
                m_Humanoid.onHealthChanged -= UpdateHealth;
                if (m_Humanoid.GetType() == typeof(Player))
                {
                    Player player = (Player)m_Humanoid;
                    player.WeaponManager.onWeaponChanged -= WeaponChanged;
                    player.WeaponManager.Weapon.onAmmoChanged -= UpdateAmmo;
                }
            }



            m_Humanoid = value;
            //subscribe
            m_Humanoid.onHealthChanged += UpdateHealth;

            //is player, subscribe to player events and set values
            if (m_Humanoid.GetType() == typeof(Player))
            {
                Player player = (Player)m_Humanoid;
                player.WeaponManager.onWeaponChanged += WeaponChanged;
                player.WeaponManager.Weapon.onAmmoChanged += UpdateAmmo;
            }
        }
    }

    void Start()
    {
        if (m_Humanoid != null)
            Humanoid = m_Humanoid;
    }

    //called on weapon change
    void WeaponChanged(Weapon weapon)
    {
        m_WeaponImage.sprite = weapon.m_UiIcon;
    }

    //called on health change
    void UpdateHealth(int newHealth)
    {
        m_HealthText.text = "" + newHealth;
    }

    void UpdateAmmo(int newAmmo)
    {
        m_AmmoText.text = "" + newAmmo;
    }

	/*void setMainWeaponImage(Sprite sprite){
		m_WeaponImage.sprite = sprite;
	}

	void setSecondWeaponImage(Sprite sprite){
		m_SecondWeaponImage.sprite = sprite;
	}*/
}
