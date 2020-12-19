using UnityEngine;

namespace Better1911
{
	public interface IConfig
	{
		float Metallic { get; }
		float NormalStrength { get; }
		float Roughness { get; }
		Color Recolor { get; }
		float Specularity { get; }
	}
}
