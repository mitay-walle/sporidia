using UnityEngine;
using UnityEngine.AI;

namespace Game
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class NavMeshAgentLinkTraveler : MonoBehaviour
	{
		private NavMeshAgent agent;
		private void Start() => agent = GetComponent<NavMeshAgent>();

		private void FixedUpdate()
		{
			if (!agent.isOnOffMeshLink) return;

			OffMeshLinkData data = agent.currentOffMeshLinkData;

			//calculate the final point of the link
			Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

			//Move the agent to the end point
			agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);

			//when the agent reach the end point you should tell it, and the agent will "exit" the link and work normally after that
			if (agent.transform.position == endPos)
			{
				agent.CompleteOffMeshLink();
			}
		}
	}
}