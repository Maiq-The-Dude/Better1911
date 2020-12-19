using BepInEx.Configuration;
using Color = UnityEngine.Color;

namespace Better1911.Configs
{
	public class GlowSightsConfig
	{
		public ConfigEntry<bool> DisableGlowSights { get; }
		public ConfigEntry<bool> CustomColor { get; }
		public ConfigEntry<Color> RearColor { get; }
		public ConfigEntry<Color> FrontColor { get; }

		public GlowSightsConfig(string section, ConfigFile config)
		{
			DisableGlowSights = config.Bind(section, nameof(DisableGlowSights), false, "Hide the glow sights");
			CustomColor = config.Bind(section, nameof(CustomColor), false, "Enable custom coloring of the glow sights");
			RearColor = config.Bind(section, nameof(RearColor), Color.blue, "Color of the rear glow sights");
			FrontColor = config.Bind(section, nameof(FrontColor), Color.yellow, "Color of the front glow sight");
		}
	}
}
