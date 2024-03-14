using Game.Systems.CombatSystem;
using System;
using UnityEngine;

namespace Game.Environment.EntitySystem
{
	public sealed class MonoBushObject : MonoObject
	{
		public MeshFilter MeshFilter;
		public float force = 10f;
		public float springForce = 20f;
		public float damping = 5f;
		
		private Mesh deformingMesh;
		private Vector3[] originalVertices;
		private Vector3[] displacedVertices;
		private Vector3[] vertexVelocities;

		private void Start()
		{
			if(!MeshFilter) return;
			deformingMesh = MeshFilter.mesh;
			originalVertices = deformingMesh.vertices;
			displacedVertices = new Vector3[originalVertices.Length];
			for ( int i = 0; i < originalVertices.Length; i++ )
			{
				displacedVertices[ i ] = originalVertices[ i ];
			}

			vertexVelocities = new Vector3[originalVertices.Length];
		}

		public override void TakeDamage( Damage damage )
		{
			if(!MeshFilter) return;

			AddDeformingForce( transform.position + damage.AttackDirection * 2f, force);
		}

		public void AddDeformingForce( Vector3 point, float force )
		{
			for ( int i = 0; i < displacedVertices.Length; i++ )
			{
				AddForceToVertex( i, point, force );
			}
		}

		private void AddForceToVertex( int i, Vector3 point, float force )
		{
			Vector3 pointToVertex = displacedVertices[i] - point;
			float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
			float velocity = attenuatedForce * Time.deltaTime;
			vertexVelocities[i] += pointToVertex.normalized * velocity;
		}

		private void Update()
		{
			if(!MeshFilter) return;

			for ( int i = 0; i < displacedVertices.Length; i++ )
			{
				UpdateVertex( i );
			}

			deformingMesh.vertices = displacedVertices;
			deformingMesh.RecalculateNormals();
		}

		private void UpdateVertex( int i )
		{
			Vector3 velocity = vertexVelocities[ i ];
			Vector3 displacement = displacedVertices[i] - originalVertices[i];
			velocity -= displacement * springForce * Time.deltaTime;
			velocity *= 1f - damping * Time.deltaTime;
			vertexVelocities[i] = velocity;
			displacedVertices[ i ] += velocity * Time.deltaTime;
		}
	}
}