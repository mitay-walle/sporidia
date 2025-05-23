using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
namespace Gameplay.Interactions.Damages
{
	[Serializable, InlineProperty]
	public class AnimatorParameter
	{
		[HideLabel] public Animator animator;

		[HideLabel, Dropdown("GetParameters")]
		public string parameter;

		public bool IsAllowed() => animator && !string.IsNullOrEmpty(parameter);

		List<string> allParameters = new();

		List<string> GetParameters()
		{
			allParameters.Clear();
			if (animator)
			{
				AnimatorControllerParameter[] parameters = animator.parameters;
				foreach (AnimatorControllerParameter parameter in parameters)
				{
					allParameters.Add(parameter.name);
				}
			}
			return allParameters;
		}
	}
}