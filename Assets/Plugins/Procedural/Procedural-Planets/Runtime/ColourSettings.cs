using UnityEngine;

namespace Sporidia.Plugins.Procedural.Procedural_Planets.Runtime
{
	[CreateAssetMenu()]
	public class ColourSettings : ScriptableObject
	{
		public Material planetMaterial;
		public BiomeColourSettings biomeColourSettings;
		[GradientUsage(true)] public Gradient oceanColour;

		[System.Serializable]
		public class BiomeColourSettings
		{
			public Biome[] biomes;
			public NoiseSettings noise;
			public float noiseOffset;
			public float noiseStrength;
			[Range(0, 1)]
			public float blendAmount;

			[System.Serializable]
			public class Biome
			{
				[GradientUsage(true)] public Gradient gradient;
				[ColorUsage(false, true)] public Color tint;
				[Range(0, 1)]
				public float startHeight;
				[Range(0, 1)]
				public float tintPercent;
			}
		}
	}
}