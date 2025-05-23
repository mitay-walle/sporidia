using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Game.Behavior
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class AgentLinkMover : MonoBehaviour
	{
		[SerializeField] private string _jumpAnimation = "Jump";
		[SerializeField] private string _idleAnimation = "Idle";
		[SerializeField] private float ScaleForHeightJumpUp = 1.5f;
		[SerializeField] private float _duration = .7f;
		[SerializeField] private float JumpUpHeight = 3.0f;
		[SerializeField] private float JumpDownHeight = 3.0f;
		public UnityEvent OnStart;
		public UnityEvent OnComplete;

		private IEnumerator Start()
		{
			NavMeshAgent agent = GetComponent<NavMeshAgent>();
			agent.autoTraverseOffMeshLink = false;
			while (true)
			{
				if (agent.isOnOffMeshLink)
				{
					yield return StartCoroutine(Jump(agent, _duration));
					GetComponentInChildren<Animator>()?.Play(_idleAnimation);
					agent.CompleteOffMeshLink();
					OnComplete?.Invoke();
				}
				yield return null;
			}
		}

		private IEnumerator Jump(NavMeshAgent agent, float duration)
		{
			GetComponentInChildren<Animator>()?.Play(_jumpAnimation);
			OnStart?.Invoke();
			OffMeshLinkData data = agent.currentOffMeshLinkData;
			Vector3 startPos = agent.transform.position;
			Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
			float height = endPos.y - startPos.y;
			float normalizedTime = 0.0f;
			// Jump to Up
			if (height >= 0)
			{
				height = JumpUpHeight + ScaleForHeightJumpUp * height;
				while (normalizedTime < 1.0f)
				{
					float yOffset = height * (normalizedTime - normalizedTime * normalizedTime);
					agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
					normalizedTime += Time.deltaTime / duration;
					yield return null;
				}
			}
			// Jump to Down
			else
			{
				height = Math.Abs(height) + JumpDownHeight;
				while (normalizedTime < 1.0f)
				{
					float yOffset = height * (normalizedTime - normalizedTime * normalizedTime);
					agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
					normalizedTime += Time.deltaTime / duration;
					yield return null;
				}
			}
		}
	}
}