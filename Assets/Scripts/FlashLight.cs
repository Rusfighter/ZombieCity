using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class FlashLight : MonoBehaviour {

	private float range;
	private Color color;
	private float angle;


	void OnEnable(){
		Shader.EnableKeyword ("IS_ADVANCED");
	}

	void OnDisable(){
		Shader.DisableKeyword ("IS_ADVANCED");
	}

	void Update(){
		Light light = GetComponent<Light> ();

		color = light.color;
		range = light.range;
		angle = light.spotAngle;
		Shader.SetGlobalVector("_FlashLight_Position", new Vector4(transform.position.x, transform.position.y, transform.position.z, 0));
		Shader.SetGlobalVector("_FlashLight_Forward", new Vector4(transform.forward.x,transform.forward.y,transform.forward.z, 0));
		Shader.SetGlobalVector("_FlashLight_Color", new Vector4(color.r,color.g,color.b,color.a)); 
		Shader.SetGlobalVector ("_FlashLight_Params", new Vector4(range, range, light.intensity, Mathf.Abs(Mathf.Cos(angle/2f * Mathf.Deg2Rad))));

		light.enabled = false;
	}
}
