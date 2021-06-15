using BepInEx.Configuration;

namespace Better1911.Configs
{
	public class MagazineConfig
	{
		public ConfigEntry<bool> FixMagPos { get; }
		public ConfigEntry<MagazineMaterial> CustomMagazineMaterial { get; }

		public MagazineConfig(string section, ConfigFile config)
		{
			FixMagPos = config.Bind(section, nameof(FixMagPos), true, "Attempts to fix the standard capacity magazine positions for M1911 Tactical & Operator");
			CustomMagazineMaterial = config.Bind(section, nameof(CustomMagazineMaterial), MagazineMaterial.Default, "Select Slide or Frame if you want your magazine material to match");
		}

		public enum MagazineMaterial
		{
			Default,
			Slide,
			Frame
		}
	}
}