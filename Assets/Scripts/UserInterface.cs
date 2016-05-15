using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using System;

public class UserInterface : MonoBehaviour
{
    public Player m_Player;
    public WeaponHandler m_WeaponHandler;
    public Text m_HealthText;
	public Text m_AmmoText;
	public Text m_WaveText;
	public Text m_ZombiesLeftText;
	public Text m_ClipSizeText;
    public Image m_WeaponImage;
	public Image m_SecondWeaponImage;
    public Text m_AnnouncementText;

	public Slider m_ClipSlider;

	const string m_WaveHolder = "Wave: {0}";
	private int m_ammoInClip;
	private int m_ClipSize;

    private bool m_IsReloading = false;

    private GameHandler.LevelState state = GameHandler.LevelState.EMPTY;

    //Tweens
    Tween m_ReloadTween;
    Tween m_AnnouncementTween;

    void Awake()
    {
        DOTween.Init();
    }

	void Start(){
		//set images, stats etc here
		/*Weapon weapon = m_WeaponHandler.getNextWeapon ().GetComponent<Weapon>();
		setSecondWeaponImage (weapon.UiIcon);*/
	}
		
	// Update is called once per frame
	void Update () {
		/*if (m_ZombiesLeft != WaveGenerator.instance.EnemiesLeft) {
			m_ZombiesLeft = WaveGenerator.instance.EnemiesLeft;
			m_ZombiesLeftText.text = m_ZombiesLeft.ToString();
		}

        if (m_Health != m_Player.Health)
        {
            m_Health = m_Player.Health;
            m_HealthText.text = ((int) m_Health).ToString();
            	
		}*/

        /*if (m_IsReloading != m_WeaponHandler.Weapon.IsReloading)
        {
            m_IsReloading = m_WeaponHandler.Weapon.IsReloading;
            if (m_ReloadTween == null || !m_ReloadTween.IsPlaying()){
                m_ReloadTween = m_WeaponImage.DOFade(0f, 0.3f).From();
                m_ReloadTween.SetLoops(int.MaxValue, LoopType.Yoyo);
            }
            else m_ReloadTween.Complete();
        }

        if (m_ammoInClip != m_WeaponHandler.Weapon.AmmoInClip)
        {
            m_ammoInClip = m_WeaponHandler.Weapon.AmmoInClip;
            m_ClipSizeText.text = m_ammoInClip.ToString();
			m_ClipSlider.value = (float) m_ammoInClip/m_WeaponHandler.Weapon.clipSize;
        }

		if (m_WeaponImage.sprite != m_WeaponHandler.Weapon.UiIcon) {
			setMainWeaponImage(m_WeaponHandler.Weapon.UiIcon);
		}*/
		
	}

	public void Reload () {
		m_WeaponHandler.ReloadWeapon ();
	}

	public void NextWeapon(Image img){
		setSecondWeaponImage (m_WeaponImage.sprite);
		m_WeaponHandler.nextWeapon ();
	}

	void setMainWeaponImage(Sprite sprite){
		m_WeaponImage.sprite = sprite;
	}

	void setSecondWeaponImage(Sprite sprite){
		m_SecondWeaponImage.sprite = sprite;
	}
        
    void SetAnnouncement(string text)
    {
        if (m_AnnouncementTween != null && m_AnnouncementTween.IsPlaying()) return;
        m_AnnouncementText.gameObject.SetActive(true);
        m_AnnouncementTween = m_AnnouncementText.DOText(text, 2f, true, ScrambleMode.Numerals).OnComplete(()=> {
            m_AnnouncementTween = m_AnnouncementText.DOFade(0, 0.3f).SetDelay(1.5f).OnComplete(()=> {
                m_AnnouncementText.DOFade(1, 0);
                m_AnnouncementText.gameObject.SetActive(false);
            });
        });
    }

    public void onNotify(GameHandler data)
    {
        m_WaveText.text = string.Format(m_WaveHolder, data.Level);

        switch (data.State)
        {
            case GameHandler.LevelState.GAME_OVER:
                SetAnnouncement("Game Over!");
                break;
            case GameHandler.LevelState.WAVE_BUSY:
                SetAnnouncement("Kill all the zombies!");
                break;
            case GameHandler.LevelState.WAVE_COMPLETED:
                SetAnnouncement("Wave completed!");
                break;
            case GameHandler.LevelState.WAVE_SETUP:
                SetAnnouncement("Prepare yourself \n Bullshit is coming!");
                break;
        }
    }
}
