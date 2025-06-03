using UnityEngine;

namespace Sporidia.Plugins.Procedural.Procedural_Planets.Runtime
{
    public interface INoiseFilter {

        float Evaluate(Vector3 point);
    }
}
