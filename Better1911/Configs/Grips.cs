using BepInEx.Configuration;
using UnityEngine;

namespace Better1911.Configs
{
	public class GripsConfig : SharedConfigs
	{
		public GripsConfig(string section, ConfigFile config)
		{
			var c = new Vector4(28, 28, 28, 1);
			Recolor = config.Bind(section, nameof(Recolor), c, "Color of the grips (RGBA)");
			RecolorIntensity = config.Bind(section, nameof(RecolorIntensity), 1f, "Color intensity of the recolor. Increase if the colors appear too dark");
			Roughness = config.Bind(section, nameof(Roughness), 0.409f, "Roughness of the material. Decrease for a more mirrored finish");
			Metallic = config.Bind(section, nameof(Metallic), 0.489f, "Metallic value for the material. Decrease for a flatter more polymer look");
			NormalStrength = config.Bind(section, nameof(NormalStrength), 1f, "Strength of the normal maps. Increase this for a more rugged look or decrease for a smoother look");
			Specularity = config.Bind(section, nameof(Specularity), 0f, "Specularity for the material");
		}
	}
}
