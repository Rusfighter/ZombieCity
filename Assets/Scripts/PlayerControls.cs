using UnityEngine;

namespace Assets.Scripts
{
    class PlayerControls : MonoBehaviour
    {
        Player player;

        void Start()
        {
            player = GetComponent<Player>();
        }

        void Update()
        {
            if (player.isDead) return;

            if (Input.GetMouseButtonDown(1))
            {
                ClickAction(Input.mousePosition);

            }
            else if (Input.touchCount > 0)
            {
                ClickAction(Input.GetTouch(0).position);
            }
        }

        void ClickAction(Vector3 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Enemy") player.Focus = hit.collider.transform;
                else player.setDestination(hit.point);
            }
        }
    }
}
