using UnityEngine;

namespace Sporidia.Plugins.Procedural.Procedural_Planets.Runtime
{
    [CreateAssetMenu()]
    public class ShapeSettings : ScriptableObject {

        public float planetRadius = 1;
        public NoiseLayer[] noiseLayers;

        [System.Serializable]
        public class NoiseLayer
        {
            public bool enabled = true;
            public bool useFirstLayerAsMask;
            public NoiseSettings noiseSettings;
        }
    }
}
