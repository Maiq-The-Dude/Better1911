using BepInEx.Configuration;
using UnityEngine;

namespace Better1911.Configs
{
	public class GlowSightsConfig
	{
		public ConfigEntry<bool> DisableGlowSights { get; }
		public ConfigEntry<bool> CustomColor { get; }
		public ConfigEntry<Vector4> RearColor { get; }
		public ConfigEntry<Vector4> FrontColor { get; }
		public ConfigEntry<float> Intensity { get; }

		public GlowSightsConfig(string section, ConfigFile config)
		{
			DisableGlowSights = config.Bind(section, nameof(DisableGlowSights), false, "Hide the glow sights");
			CustomColor = config.Bind(section, nameof(CustomColor), false, "Enable custom coloring of the glow sights");

			var c = new Vector4(255, 0, 0, 1);
			RearColor = config.Bind(section, nameof(RearColor), c, "Color of the rear glow sights (RGBA)");
			FrontColor = config.Bind(section, nameof(FrontColor), c, "Color of the front glow sight (RGBA)");
			Intensity = config.Bind(section, nameof(Intensity), 1.0f, "Intensity of the glow");
		}
	}
}