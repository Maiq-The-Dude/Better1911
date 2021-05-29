using BepInEx.Configuration;
using UnityEngine;

namespace Better1911.Configs
{
	public class FrameConfig : SharedConfigs
	{
		public FrameConfig(string section, ConfigFile config)
		{
			var c = new Vector4(255, 255, 255, 1);
			Recolor = config.Bind(section, nameof(Recolor), c, "Color of the frame (RGBA)");
			RecolorIntensity = config.Bind(section, nameof(RecolorIntensity), 1f, "Color intensity of the recolor. Increase if the colors appear too dark");
			Roughness = config.Bind(section, nameof(Roughness), 1f, "Roughness of the material. Decrease for a more mirrored finish.");
			Metallic = config.Bind(section, nameof(Metallic), 1f, "Metallic value for the material. Decrease for a flatter more polymer look.");
			NormalStrength = config.Bind(section, nameof(NormalStrength), 1f, "Strength of the normal maps. Increase this for a more rugged look or decrease for a smoother look.");
			Specularity = config.Bind(section, nameof(Specularity), 1f, "Specularity for the material.");
			RemoveTexture = config.Bind(section, nameof(RemoveTexture), false, "Enable to remove texture. This can allow cleaner finishes and better recoloring.");
		}
	}
}