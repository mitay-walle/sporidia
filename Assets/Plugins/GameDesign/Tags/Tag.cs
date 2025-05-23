using TriInspector;
using UnityEngine;

namespace Design.Tags
{
	[CreateAssetMenu(menuName = "Tags/Tag")]
	public class Tag : ScriptableObject
	{
		[field: SerializeField] public Color Color { get; private set; } = Color.white;
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public string Description { get; private set; }
		[ShowInInspector, TextArea] protected string _finalText => ToString();

		[Button]
		public override string ToString() => $"<color=#{ColorUtility.ToHtmlStringRGBA(Color)}>{Name}</color> - {Description}";
	}
}