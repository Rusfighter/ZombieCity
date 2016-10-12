using UnityEngine;

public class Player : Humanoid
{
    [SerializeField]
    private Animator m_CharAnimator;
    [SerializeField]
    private WeaponManager m_WeaponHandler;

    private Rigidbody m_Rigidbody;

    public InputHandler m_Input;

    public Camera m_Cam;

    public WeaponManager WeaponManager
    {
        get { return m_WeaponHandler; }
    }
    
    public Rigidbody rb
    {
        get { return m_Rigidbody; }
    }

    protected override void OnAwakeAction()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Input = new InputHandler();
    }

    protected override void OnStartAction()
    {

    }

    void Update() {
        if (isDead) return;

        //commands
        Move(m_Input.getAxis());
        Rotate(m_Input.getDirection(m_Cam, transform));
        Command<Player> command = m_Input.handleButtons();
        if (command != null)
            command.Execute(this);
	}

    public override void Move(Vector2 dir)
    {
        base.Move(dir);
        MoveAnimation(dir);
    }
    
    void MoveAnimation(Vector2 dir) {
        Vector3 lookTo = transform.forward.normalized;
        lookTo.y = 0;
        Vector3 moveDirection = new Vector3(dir.x, 0, dir.y);
        moveDirection.y = 0;

        float angle = Vector3.Angle(moveDirection, lookTo) * Mathf.Deg2Rad;
        m_CharAnimator.SetFloat("Forward", Mathf.Cos(angle) );

        lookTo = Vector3.Cross(lookTo, Vector3.up);
        angle = Vector3.Angle(moveDirection, lookTo) * Mathf.Deg2Rad;
        m_CharAnimator.SetFloat("Turn", Mathf.Cos(angle));
    }

    public void Jump()
    {
        Debug.Log("jump");
    }


    protected override void OnDeathAction()
    {
        //throw new NotImplementedException();
    }

    protected override void OnDamageAction(float damage)
    {
        //throw new NotImplementedException();
    }
}
