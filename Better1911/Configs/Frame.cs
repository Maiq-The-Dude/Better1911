using BepInEx.Configuration;
using UnityEngine;

namespace Better1911.Configs
{
	public class FrameConfig : SharedConfigs
	{
		public FrameConfig(string section, ConfigFile config)
		{
			Color c = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
			Recolor = config.Bind(section, nameof(Recolor), c, "Color of the frame");
			Roughness = config.Bind(section, nameof(Roughness), 1f, "Roughness of the material. Decrease for a more mirrored finish.");
			Metallic = config.Bind(section, nameof(Metallic), 1f, "Metallic value for the material. Decrease for a flatter more polymer look.");
			NormalStrength = config.Bind(section, nameof(NormalStrength), 1f, "Strength of the normal maps. Increase this for a more rugged look or decrease for a smoother look.");
			Specularity = config.Bind(section, nameof(Specularity), 0f, "Specularity for the material.");
			RemoveTexture = config.Bind(section, nameof(RemoveTexture), false, "Enable to remove texture. This can allow cleaner finishes and better recoloring.");
		}
	}
}
