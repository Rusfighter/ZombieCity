using UnityEngine;
namespace Assets.Scripts
{
    [System.Serializable]
    public class Weapon
    {
        public GameObject obj;
        public int damage = 10;
        public float range = 8f;
        public float shootTime = 0.2f;
        public int animation = 1;
    }
}
