using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class DynamicBatcher : MonoBehaviour {

	public List<Transform> gameObjects;

	private List<Bounds> objectBounds = new List<Bounds>();

	protected MeshFilter cachedMeshFilter;

	private Renderer objectRenderer = null;

	public bool Visible { get; private set; }

	private string[] matrices;


	void Awake(){

		//return;
		// Default visibility
		Visible = false;

		cachedMeshFilter = GetComponent<MeshFilter>();
		objectRenderer = GetComponent<MeshRenderer>();

		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
		for (int i = 1; i < renderers.Length; i++) {
			gameObjects.Add(renderers[i].transform);
		}

		objectRenderer.sharedMaterial = renderers [1].sharedMaterial;

		Batch();

	}

	public void OnBecameVisible(){
		Visible = true;
		UpdateShaderParameters();
	}
	public void OnBecameInvisible() { Visible = false; }
	
	public void LateUpdate()
	{
		if (!Visible) return;
		CalculateBoundingBox();
	}
	
	public void OnWillRenderObject()
	{
		UpdateShaderParameters();
	}

	void Batch(){
		if (gameObjects.Count == 0) return;

		// Make a mesh list for batching
		List<Mesh> meshList = new List<Mesh>();

		matrices = new string[gameObjects.Count];

		for (int i = 0; i<gameObjects.Count; i++) {
			Transform currentObject = gameObjects[i];
			MeshFilter filter = currentObject.GetComponent<MeshFilter>();
			MeshRenderer renderer = currentObject.GetComponent<MeshRenderer>();
			renderer.enabled = false;


			if (filter != null){
				meshList.Add(filter.sharedMesh);
				objectBounds.Add(filter.sharedMesh.bounds);
			}
			else{
				meshList.Add(new Mesh());
				objectBounds.Add(new Bounds());
			}

			matrices[i] = "ModelMatrix"+i;
		}
		
		Mesh mesh = cachedMeshFilter.sharedMesh;
		// If we had a previous batch, destroy it
		if (mesh != null) Destroy(mesh);
		
		// Get new the new batched mesh
		cachedMeshFilter.sharedMesh = BatchList(meshList);
		
		// Update material (instantiate a new material) same as objectRenderer.renderer
		objectRenderer.sharedMaterial = new Material(objectRenderer.sharedMaterial);
	}

	public static Mesh BatchList(List<Mesh> meshList)
	{
		// We will use this to combine the meshes
		CombineInstance[] combineInstances = new CombineInstance[meshList.Count];
		List<Vector4> tangentSelectors = new List<Vector4>();
		
		for (int i = 0; i < combineInstances.Length; i++)
		{
			Mesh mesh = meshList[i];
			
			for (int j = 0; j < mesh.vertexCount; j++)
			{
				// Add selector data
				tangentSelectors.Add(new Vector4(i, 0, 0, 0));
			}
			
			combineInstances[i].mesh = mesh;
			
			// We don't want to transform the meshes, it will be done in the shader.
			//combineInstances[i].transform = Matrix4x4.identity;
		}
		
		// Make our new mesh out of the combined instance
		Mesh resultMesh = new Mesh();
		resultMesh.CombineMeshes(combineInstances, true, false);
		// And set the selectors of the meshes
		resultMesh.tangents = tangentSelectors.ToArray();
		
		return resultMesh;
	}

	protected virtual void UpdateShaderParameters()
	{
		for (int i = 0; i < gameObjects.Count; i++)
		{
			objectRenderer.sharedMaterial.SetMatrix(matrices[i], gameObjects[i].transform.localToWorldMatrix);
		}
	}

	protected virtual void CalculateBoundingBox()
	{
		// Nothing to Calculate
		if (objectBounds.Count == 0)
		{
			cachedMeshFilter.sharedMesh.bounds = new Bounds();
			return;
		}
		
		// Take the first object for the starting point of the calculation
		Bounds newBounds = objectBounds[0];
		
		// Calculate all other objects
		for (int i = 1; i < objectBounds.Count; i++)
		{
			newBounds.Encapsulate(new Bounds(transform.InverseTransformPoint(gameObjects[i].position), objectBounds[i].size));
		}
		
		// Set the new bounding box to be used
		cachedMeshFilter.sharedMesh.bounds = newBounds;
		//Debug.Log (newBounds.ToString());
	}	
}
