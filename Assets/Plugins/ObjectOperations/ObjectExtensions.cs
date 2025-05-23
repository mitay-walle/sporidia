using UnityEditor;
using UnityEngine;

namespace GameJam.Plugins.ObjectOperations
{
	public static class ObjectExtensions
	{
#if UNITY_EDITOR
		[MenuItem("CONTEXT/Object/copy type name")]
		public static void CopyTypeName(MenuCommand command)
		{
			GUIUtility.systemCopyBuffer = command.context.GetType().Name;
		}

		[MenuItem("Assets/Reserialize")]
		public static void Reserialize()
		{
			foreach (Object obj in Selection.objects)
			{
				if (EditorUtility.IsPersistent(obj))
				{
					AssetDatabase.ForceReserializeAssets(new[] { AssetDatabase.GetAssetPath(obj) });
				}
			}
		}
#endif
	}
}