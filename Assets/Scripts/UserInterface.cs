using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class UserInterface : MonoBehaviour {
		private Text Health;
		private Text Ammo;
		private int AmmoInClip;
		private int ClipSize;
		public Player player;

		void Start () {
			Health.text = "100";
			Ammo.text = "12/30"; //Ammo in clip / Clipsize
			// En dit zal later natuurlijk AmmoInClip/TotalAmmo zijn
		}
		
		// Update is called once per frame
		void Update () {
			Health.text = player.Health.ToString ("F0");
		
		}
	}
}