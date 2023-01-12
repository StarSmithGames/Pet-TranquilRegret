using Game.Entities;
using UnityEngine;

using Zenject;

public class Player : MonoBehaviour
{
	public Transform Model => model;
	[SerializeField] private Transform model;

	public Transform CameraFollowPivot => cameraFollowPivot;
	[SerializeField] private Transform cameraFollowPivot;

	public Transform CameraLookAtPivot => cameraLookAtPivot;
    [SerializeField] private Transform cameraLookAtPivot;

	public PlayerVFX PlayerVFX => playerVFX;
	[SerializeField] private PlayerVFX playerVFX;

	public PlayerCanvas PlayerCanvas => playerCanvas;
	[SerializeField] private PlayerCanvas playerCanvas;

	private void OnDrawGizmos()
	{
		if (cameraLookAtPivot != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(cameraLookAtPivot.position, 0.2f);
		}

		if (cameraFollowPivot != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(cameraFollowPivot.position, 0.2f);
		}
	}

	public class Factory : PlaceholderFactory<Player> { }
}