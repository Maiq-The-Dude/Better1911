using BepInEx.Configuration;
using UnityEngine;

namespace Better1911.Configs
{
	public abstract class SharedConfigs : IConfig
	{
		public ConfigEntry<Vector4> Recolor { get; set; }
		public ConfigEntry<float> Roughness { get; set; }
		public ConfigEntry<float> Metallic { get; set; }
		public ConfigEntry<float> NormalStrength { get; set; }
		public ConfigEntry<float> Specularity { get; set; }
		public ConfigEntry<bool> RemoveTexture { get; set; }
		public ConfigEntry<float> RecolorIntensity { get; set; }

		Vector4 IConfig.Recolor => Recolor.Value;
		float IConfig.Roughness => Roughness.Value;
		float IConfig.Metallic => Metallic.Value;
		float IConfig.NormalStrength => NormalStrength.Value;
		float IConfig.Specularity => Specularity.Value;
		float IConfig.RecolorIntensity => RecolorIntensity.Value;
	}
}