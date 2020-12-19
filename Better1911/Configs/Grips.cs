using BepInEx.Configuration;
using UnityEngine;

namespace Better1911.Configs
{
	public class GripsConfig : SharedConfigs
	{
		public GripsConfig(string section, ConfigFile config)
		{
			Color c = new Color32(0x1C, 0x1C, 0x1C, 0xFF);
			Recolor = config.Bind(section, nameof(Recolor), c, "Color of the grips");
			Roughness = config.Bind(section, nameof(Roughness), 0.409f, "Roughness of the material. Decrease for a more mirrored finish.");
			Metallic = config.Bind(section, nameof(Metallic), 0.489f, "Metallic value for the material. Decrease for a flatter more polymer look.");
			NormalStrength = config.Bind(section, nameof(NormalStrength), 1f, "Strength of the normal maps. Increase this for a more rugged look or decrease for a smoother look.");
			Specularity = config.Bind(section, nameof(Specularity), 0f, "Specularity for the material.");			
		}
	}
}
