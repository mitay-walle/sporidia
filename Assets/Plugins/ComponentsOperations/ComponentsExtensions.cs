using UnityEngine;

namespace Plugins.ComponentsOperations
{
	public static class ComponentsExtensions
	{
		public static T CheckComponent<T>(this GameObject gameObject) where T : Component
		{
			T component = gameObject.GetComponent<T>();
			if (component != null)
			{
				return component;
			}

			return gameObject.AddComponent<T>();
		}
	}
}