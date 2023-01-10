using Game.Entities;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

public class Player : MonoBehaviour
{
    public Transform CameraPivot => cameraPivot;
    [SerializeField] private Transform cameraPivot;

	public PlayerVFX PlayerVFX => playerVFX;
	[SerializeField] private PlayerVFX playerVFX;

	private void OnDrawGizmos()
	{
		if (cameraPivot == null) return;

		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(cameraPivot.position, 0.1f);
	}

	public class Factory : PlaceholderFactory<Player> { }
}