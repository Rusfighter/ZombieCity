using UnityEngine;

public class InputHandler {

    const string m_Horizontal = "Horizontal";
    const string m_Vertical = "Vertical";
    float m_CamRayLength = 100f;
    int m_FloorMask;

    PlayerCommand m_ShootCommand = new ShootCommand();
    PlayerCommand m_JumpCommand = new JumpCommand();
    PlayerCommand m_NextWeapon = new NextWeaponCommand();


    public InputHandler()
    {
        m_FloorMask = LayerMask.GetMask("ground");
    }

    public Vector2 getAxis()
    {
        Vector2 vec;
        vec.x = Input.GetAxis(m_Horizontal);
        vec.y = Input.GetAxis(m_Vertical);

        return vec;
    }

    public Vector3 getDirection(Camera cam, Transform transform)
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, m_CamRayLength, m_FloorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            return playerToMouse.normalized;
        }

        //return the normal
        return transform.forward;
    }

    //buttons
    public PlayerCommand handleButtons()
    {
        if (Input.GetButton("Fire1")) return m_ShootCommand;
        if (Input.GetButtonDown("Fire2")) return m_NextWeapon;
        if (Input.GetButtonDown("Jump")) return m_JumpCommand;
        return null;
    }

}
