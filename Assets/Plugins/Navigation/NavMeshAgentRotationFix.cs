using UnityEngine;
using UnityEngine.AI;

namespace GameJam.Plugins.Navigation
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class NavMeshAgentRotationFix : MonoBehaviour
	{
		private void Awake()
		{
			GetComponent<NavMeshAgent>().updateRotation = false;
			GetComponent<NavMeshAgent>().updateUpAxis = false;
			GetComponent<NavMeshAgent>().angularSpeed = 0;
			transform.rotation = Quaternion.identity;
		}
	}
}