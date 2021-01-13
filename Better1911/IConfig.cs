using UnityEngine;

namespace Better1911
{
	public interface IConfig
	{
		float Metallic { get; }
		float NormalStrength { get; }
		float Roughness { get; }
		Vector4 Recolor { get; }
		float Specularity { get; }
		float RecolorIntensity { get; }
	}
}
