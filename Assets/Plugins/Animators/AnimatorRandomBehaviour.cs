using System.Collections.Generic;
using System.Linq;
using GameJam.Plugins.Audio;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif
namespace Plugins.Runtime.Animators
{
	public class AnimatorRandomBehaviour : StateMachineBehaviour, ISerializationCallbackReceiver
	{
		[SerializeField] RuntimeAnimatorController animatorController;
		[SerializeField] bool isActive;
		[SerializeField] int myLayerIndex;
		[SerializeField] float crossfadeTime = .1f;
		[SerializeField] List<int> statesNames = new();
		bool isCrossFading;
		int isCrossFadingFromThis;


		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!isActive) return;
			if (isCrossFading) return;
			if (myLayerIndex != layerIndex) return;

			int randomNameHash = statesNames.Random();
			if (randomNameHash != stateInfo.shortNameHash)
			{
				animator.CrossFade(randomNameHash, crossfadeTime, layerIndex);
				isCrossFadingFromThis = stateInfo.fullPathHash;
			}

		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (!isActive) return;

			if (isCrossFadingFromThis == stateInfo.shortNameHash)
			{
				isCrossFading = false;
				isCrossFadingFromThis = -1;
			}
		}

		void CollectStates()
		{
#if UNITY_EDITOR
			if (Application.isPlaying) return;
			if (!animatorController) return;
			var controller = animatorController as AnimatorController;
			AnimatorStateMachine machine = null;
			for (var i = 0; i < controller.layers.Length; i++)
			{
				AnimatorControllerLayer layer = controller.layers[i];
				machine = layer.stateMachine.stateMachines.ToList().Find(childMachine =>
				{
					if (childMachine.stateMachine.behaviours.ToList().Contains(this))
					{
						return true;
					}
					return false;
				}).stateMachine;
				myLayerIndex = i;
				if (machine) break;
			}
			statesNames.Clear();
			if (machine)
			{
				foreach (ChildAnimatorState childAnimatorState in machine.states)
				{
					statesNames.Add(childAnimatorState.state.nameHash);
				}
			}

			EditorUtility.SetDirty(this);
  #endif
		}
		public void OnBeforeSerialize()
		{
			CollectStates();
		}
		public void OnAfterDeserialize()
		{

		}
	}
}