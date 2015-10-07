using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class UserInterface : MonoBehaviour {
		public Text Health;
		public Text Ammo;
		private int ammoInClip;
		private int clipSize;

        private float health;
        private int wave;
        private int zombiesLeft;

        private Player player;
        private WeaponHandler weaponHandler;

        void Awake()
        {
            PlayerControls controls = FindObjectOfType<PlayerControls>();
            player = controls.gameObject.GetComponent<Player>();
            weaponHandler = controls.gameObject.GetComponent<WeaponHandler>();
        }

		void Start () {
			Health.text = "100";
			Ammo.text = "12/30"; //Ammo in clip / Clipsize
			// En dit zal later natuurlijk AmmoInClip/TotalAmmo zijn
		}
		
		// Update is called once per frame
		void Update () {

            //
            if (health != player.Health)
            {
                health = player.Health;
                Health.text = ((int) health).ToString();
            }

            if (ammoInClip != weaponHandler.Weapon.AmmoInClip)
            {
                ammoInClip = weaponHandler.Weapon.AmmoInClip;
                Ammo.text = ammoInClip.ToString();
            }
		
		}
	}
}