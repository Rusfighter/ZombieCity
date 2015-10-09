using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace Assets.Scripts
{
	public class UserInterface : MonoBehaviour {
		public Text Health;
		public Text Ammo;
		public Text Wave;
		public Text ZombiesLeft;
		public Text AmmoInClip;
        public Image WeaponImage;

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

        //Tweens
        Tween reloadTween;
	
        void Awake()
        {
            PlayerControls controls = FindObjectOfType<PlayerControls>();
            player = controls.gameObject.GetComponent<Player>();
            weaponHandler = controls.gameObject.GetComponent<WeaponHandler>();

            DOTween.Init();

        }

		void Start () {
			//Wave.text = "100";
			// En dit zal later natuurlijk AmmoInClip/TotalAmmo zijn
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
		
		}
		public void Reload () {
			weaponHandler.ReloadWeapon ();
		}
	}

}