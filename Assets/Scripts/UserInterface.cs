using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace Assets.Scripts
{
	public class UserInterface : MonoBehaviour {
		public static UserInterface instance;
		public Text Health;
		public Text Ammo;
		public Text Wave;
		public Text ZombiesLeft;
		public Text AmmoInClip;
        public Image WeaponImage;
		public Image SecondWeaponImage;
        public Text Announcement;

		public Slider ClipSlider;

		const string WaveText = "Wave: {0}";
		private int ammoInClip;
		private int clipSize;

        private float health;
        private int wave;
        private int zombiesLeft;

        private bool isReloading = false;

        private Player player;
        private WeaponHandler weaponHandler;

        private GameHandler.LevelState state = GameHandler.LevelState.EMPTY;

        //Tweens
        Tween reloadTween;
        Tween announcementTween;
	
        void Awake()
        {
			if (instance == null)
			{
				if (FindObjectsOfType(GetType()).Length > 1)
				{
					Debug.LogError("To many instances of " + GetType());
					return;
				} else instance = this;
			}

            PlayerControls controls = FindObjectOfType<PlayerControls>();
            player = controls.gameObject.GetComponent<Player>();
            weaponHandler = controls.gameObject.GetComponent<WeaponHandler>();

            DOTween.Init();
        }

		void Start(){
			//set images, stats etc here
			Weapon weapon = weaponHandler.getNextWeapon ().GetComponent<Weapon>();
			setSecondWeaponImage (weapon.UiIcon);
		}
		
		// Update is called once per frame
		void Update () {
			if (zombiesLeft != WaveGenerator.instance.EnemiesLeft) {
				zombiesLeft = WaveGenerator.instance.EnemiesLeft;
				ZombiesLeft.text = zombiesLeft.ToString();
			}
			if (wave != GameHandler.instance.Level) {
				wave = GameHandler.instance.Level;
				Wave.text = string.Format(WaveText, wave);

			}
            if (health != player.Health)
            {
                health = player.Health;
                Health.text = ((int) health).ToString();
            	
			}

            if (state != GameHandler.instance.State)
            {
                state = GameHandler.instance.State;
                switch (state)
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
                        SetAnnouncement("Prepare yourself \n Bullshit is comming!");
                        break;
                }
            }

            if (isReloading != weaponHandler.Weapon.IsReloading)
            {
                isReloading = weaponHandler.Weapon.IsReloading;
                if (reloadTween == null || !reloadTween.IsPlaying()){
                    reloadTween = WeaponImage.DOFade(0f, 0.3f).From();
                    reloadTween.SetLoops(int.MaxValue, LoopType.Yoyo);
                }
                else reloadTween.Complete();
            }

            if (ammoInClip != weaponHandler.Weapon.AmmoInClip)
            {
                ammoInClip = weaponHandler.Weapon.AmmoInClip;
                AmmoInClip.text = ammoInClip.ToString();
				ClipSlider.value = (float) ammoInClip/weaponHandler.Weapon.clipSize;
            }

			if (WeaponImage.sprite != weaponHandler.Weapon.UiIcon) {
				setMainWeaponImage(weaponHandler.Weapon.UiIcon);
			}
		
		}
		public void Reload () {
			weaponHandler.ReloadWeapon ();
		}

		public void NextWeapon(Image img){
			setSecondWeaponImage (WeaponImage.sprite);
			weaponHandler.nextWeapon ();
		}

		void setMainWeaponImage(Sprite sprite){
			WeaponImage.sprite = sprite;
		}

		void setSecondWeaponImage(Sprite sprite){
			SecondWeaponImage.sprite = sprite;
		}
        
        void SetAnnouncement(string text)
        {
            if (announcementTween != null && announcementTween.IsPlaying()) return;
            Announcement.gameObject.SetActive(true);
            announcementTween = Announcement.DOText(text, 2f, true, ScrambleMode.Numerals).OnComplete(()=> {
                announcementTween = Announcement.DOFade(0, 0.3f).SetDelay(1.5f).OnComplete(()=> {
                    Announcement.DOFade(1, 0);
                    Announcement.gameObject.SetActive(false);
                });
            });
        }
	}

}