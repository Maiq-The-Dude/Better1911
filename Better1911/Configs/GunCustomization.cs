using BepInEx.Configuration;

namespace Better1911.Configs
{
	public class GunCustomizationConfig
	{
		public ConfigEntry<bool> CustomMaterials { get; }

		public FrameConfig Frame { get; }
		public SlideConfig Slide { get; }
		public GripsConfig Grips { get; }		

		public GunCustomizationConfig(string section, ConfigFile config)
		{
			CustomMaterials = config.Bind(section, nameof(CustomMaterials), false, "Enable material editing");

			Slide = new SlideConfig(nameof(Slide), config);
			Frame = new FrameConfig(nameof(Frame), config);
			Grips = new GripsConfig(nameof(Grips), config);
		}
	}
}
