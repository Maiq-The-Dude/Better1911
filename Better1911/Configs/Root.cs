using BepInEx.Configuration;

namespace Better1911.Configs
{
	public class RootConfig
	{
		public ConfigEntry<bool> DisableRedDot { get; }
		public GlowSightsConfig GlowSights { get; }
		public GunCustomizationConfig GunCustomization { get; }
		public MagazineConfig Magazine { get; }

		public RootConfig(ConfigFile config)
		{
			DisableRedDot = config.Bind("Red Dot", nameof(DisableRedDot), true, "Hide the red dot");

			GlowSights = new GlowSightsConfig(nameof(GlowSights), config);
			GunCustomization = new GunCustomizationConfig(nameof(GunCustomization), config);
			Magazine = new MagazineConfig(nameof(Magazine), config);
		}
	}
}
