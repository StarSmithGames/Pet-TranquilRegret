using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class ParticleShaderWorldSpaceOffset : MonoBehaviour
{
	[SerializeField] private Renderer renderer;

	void Update()
	{
		Vector3 existingWorldPos = renderer.sharedMaterial.GetVector("_WorldPos");
		if (existingWorldPos != transform.position)
		{
			renderer.sharedMaterial.SetVector("_WorldPos", transform.position);
		}
	}
}