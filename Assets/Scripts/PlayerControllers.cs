using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(WeaponHandler))]
public class PlayerControllers : MonoBehaviour {
    private Player m_Player;

    private float m_CamRayLength = 100f;          // The length of the ray from the camera into the scene.
    private int m_FloorMask;

    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_FloorMask = LayerMask.GetMask("ground");
    }
	
	// Update is called once per frame
	private void Update () {
        if (m_Player.isDead)
            return;

        Moving();
        Turning();
    }

    private void Moving()
    {
        Vector3 direction = new Vector3(GetAxis("Horizontal"), 0, GetAxis("Vertical"));
        m_Player.SetMovementDirection(direction);
    }

    private void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, m_CamRayLength, m_FloorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            m_Player.SetForward(playerToMouse.normalized);

        }
    }

    protected float GetAxis(string axis)
    {
        return Input.GetAxis(axis);
    }
}
