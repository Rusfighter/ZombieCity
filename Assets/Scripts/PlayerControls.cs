using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(WeaponHandler))]
    public class PlayerControls : MonoBehaviour
    {
        Player player;
        WeaponHandler weaponHandler;
        EventSystem eventSystem;
        bool isOnUI = false;

        void Awake()
        {
            if (FindObjectsOfType(GetType()).Length > 1){
                Debug.LogError("To many instances of " + GetType());
                return;
            }

            player = GetComponent<Player>();
            weaponHandler = GetComponent<WeaponHandler>();
            eventSystem = EventSystem.current;
        }

        void Start()
        {
            weaponHandler.setWeapon(1);
        }

        void Update()
        {
            if (player.isDead) return;

            if (Input.GetMouseButtonDown(1))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                    ClickAction(Input.mousePosition);

            }
            else if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began) {
                    isOnUI = eventSystem.IsPointerOverGameObject(touch.fingerId);
                    if (!isOnUI) ClickAction(touch.position);
                }
                else if (touch.phase == TouchPhase.Moved)
                    if (!isOnUI) ClickAction(touch.position);
                else isOnUI = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                weaponHandler.nextWeapon();
            }
        }

        void ClickAction(Vector3 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("EnemyRayCast")) player.Focus = hit.collider.transform.parent;
                else player.setDestination(hit.point);
            }
        }
    }
}
