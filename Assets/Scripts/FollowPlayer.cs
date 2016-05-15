using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public Transform m_Target;            // The position that that camera will be following.
	public float m_Smoothing = 5f;        // The speed with which the camera will be following.
	
	Vector3 m_Offset;                     // The initial offset from the target.
	
	void Start ()
	{
		// Calculate the initial offset.
		m_Offset = transform.position - m_Target.position;
	}
	
	void Update ()
	{
		// Create a postion the camera is aiming for based on the offset from the target.
		Vector3 targetCamPos = m_Target.position + m_Offset;
		
		// Smoothly interpolate between the camera's current position and it's target position.
		transform.position = Vector3.Lerp (transform.position, targetCamPos, m_Smoothing * Time.deltaTime);
	}
}
