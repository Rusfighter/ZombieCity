using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class UserInterface : MonoBehaviour {
		public Text Health;
		public Text Ammo;
		public Player player;

		void Start () {
			Health.text = "100";
			Ammo.text = "30/90";
		}
		
		// Update is called once per frame
		void Update () {
			Health.text = player.Health.ToString ("F0");
		
		}
	}
}