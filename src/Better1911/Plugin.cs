using BepInEx;
using Better1911.Configs;
using FistVR;
using UnityEngine;

namespace Better1911
{
	[BepInPlugin("maiq.Better1911", "Better1911", "1.5.1")]
	[BepInProcess("h3vr.exe")]
	public class Plugin : BaseUnityPlugin
	{
		private RootConfig _configs { get; }

		private const string _tacID = "M1911Tactical";
		private const string _tacMagID = "MagazineM1911Tactical";

		public Plugin()
		{
			_configs = new RootConfig(Config);
			Hook();
		}

		private void Hook()
		{
			On.FistVR.HandgunSlide.Awake += HandgunSlide_Awake;
			On.FistVR.FVRFireArmMagazine.Load += FVRFireArmMagazine_Load;
			On.FistVR.FVRFireArmMagazine.Release += FVRFireArmMagazine_Release;
		}

		private void Unhook()
		{
			On.FistVR.HandgunSlide.Awake -= HandgunSlide_Awake;
			On.FistVR.FVRFireArmMagazine.Load -= FVRFireArmMagazine_Load;
			On.FistVR.FVRFireArmMagazine.Release -= FVRFireArmMagazine_Release;
		}

		private void OnDestroy()
		{
			Unhook();
		}

		private void HandgunSlide_Awake(On.FistVR.HandgunSlide.orig_Awake orig, FistVR.HandgunSlide self)
		{
			orig(self);

			var obj = self.Handgun.ObjectWrapper;
			if (obj?.ItemID == _tacID)
			{
				Config.Reload();
				var slideTF = self.transform;

				// Red Dot
				if (_configs.DisableRedDot.Value)
				{
					slideTF.Find("Sight").gameObject.SetActive(false);
				}

				// Glow Sights
				var glowCfg = _configs.GlowSights;
				var glowsights = new Transform[] { slideTF.Find("GlowSight (1)"), slideTF.Find("GlowSight (2)"), slideTF.Find("GlowSight (3)") };
				if (glowCfg.DisableGlowSights.Value)
				{
					foreach (var sight in glowsights)
					{
						sight.gameObject.SetActive(false);
					}
				}
				else if (glowCfg.CustomColor.Value)
				{
					for (var i = 0; i < glowsights.Length; i++)
					{
						var mat = glowsights[i].GetComponent<Renderer>().material;
						if (i == glowsights.Length - 1)
						{
							mat.SetColor("_Color", Recolor(glowCfg.FrontColor.Value, 1f));
							mat.SetColor("_EmissionColor", Recolor(glowCfg.FrontColor.Value, glowCfg.Intensity.Value));
						}
						else
						{
							mat.SetColor("_Color", Recolor(glowCfg.RearColor.Value, 1f));
							mat.SetColor("_EmissionColor", Recolor(glowCfg.RearColor.Value, glowCfg.Intensity.Value));
						}
					}
				}

				// Gun Materials
				var gunCfg = _configs.GunCustomization;
				if (gunCfg.CustomMaterials.Value)
				{
					var gun = slideTF.parent;

					// Frame
					var frameMat = gun.transform.Find("Frame").GetComponent<Renderer>().material;
					var frameCfg = gunCfg.Frame;
					if (frameCfg.RemoveTexture.Value)
					{
						frameMat.SetTexture("_MainTex", null);
					}

					PaintComponent(frameMat, frameCfg);

					// Grip
					var gripsMat = gun.transform.Find("Grips").GetComponent<Renderer>().material;
					PaintComponent(gripsMat, gunCfg.Grips);

					// Slide
					var slideMat = slideTF.Find("Slide").GetComponent<Renderer>().material;
					var slideCfg = gunCfg.Slide;
					if (slideCfg.RemoveTexture.Value)
					{
						slideMat.SetTexture("_MainTex", null);
					}

					PaintComponent(slideMat, slideCfg);
				}
			}
		}

		private void FVRFireArmMagazine_Load(On.FistVR.FVRFireArmMagazine.orig_Load orig, FistVR.FVRFireArmMagazine self, FistVR.FVRFireArm fireArm)
		{
			orig(self, fireArm);

			if (Better1911(self.FireArm))
			{
				Config.Reload();

				var tf = self.transform;
				var obj = self.ObjectWrapper;

				// Magic numbers to fix low cap mag pose/scale
				if (IsLowCapMag(self))
				{
					tf.localScale = new Vector3(1f, 1.07f, 1f);
					tf.localPosition += new Vector3(0, -0.0052f, 0);
				}

				// Magazine Materials
				if (self.FireArm.ObjectWrapper.ItemID == _tacID)
				{
					var cfg = _configs.Magazine;
					if (_configs.GunCustomization.CustomMaterials.Value && cfg.CustomMagazineMaterial.Value != MagazineConfig.MagazineMaterial.Default)
					{
						var viz = tf.Find("Viz");
						if (viz != null)
						{
							Material mat;
							for (int i = 0; i < viz.childCount; i++)
							{
								var name = viz.GetChild(i).name.ToUpper();
								if (name.Contains("MAG") || name.Contains("GEO"))
								{
									mat = viz.GetChild(i).GetComponent<Renderer>().material;
									switch (cfg.CustomMagazineMaterial.Value)
									{
										case (MagazineConfig.MagazineMaterial.Frame):
											PaintMag(mat, _configs.GunCustomization.Frame, obj.ItemID);
											break;

										case (MagazineConfig.MagazineMaterial.Slide):
											PaintMag(mat, _configs.GunCustomization.Slide, obj.ItemID);
											break;

										default:
											break;
									}

									break;
								}
							}
						}
					}
				}
			}
		}

		private void FVRFireArmMagazine_Release(On.FistVR.FVRFireArmMagazine.orig_Release orig, FVRFireArmMagazine self, bool PhysicalRelease)
		{
			if (Better1911(self.FireArm) && IsLowCapMag(self))
			{
				self.transform.localScale = new Vector3(1f, 1f, 1f);
			}

			orig(self, PhysicalRelease);
		}

		#region Helpers

		private void PaintMag(Material mat, IConfig config, string id)
		{
			if (id == _tacMagID)
			{
				mat.SetFloat("_Metal", config.Metallic);
				mat.SetFloat("_BumpScale", config.NormalStrength);
				mat.SetColor("_Color", Recolor(config.Recolor, config.RecolorIntensity));
				mat.SetFloat("_Specularity", config.Specularity);
			}
			else
			{
				if (_configs.GunCustomization.Frame.RemoveTexture.Value)
				{
					mat.SetTexture("_MainTex", null);
				}
				PaintComponent(mat, config);
			}
		}

		private void PaintComponent(Material mat, IConfig config)
		{
			mat.SetFloat("_Metal", config.Metallic);
			mat.SetFloat("_BumpScale", config.NormalStrength);
			mat.SetFloat("_Roughness", config.Roughness);
			mat.SetColor("_Color", Recolor(config.Recolor, config.RecolorIntensity));
			mat.SetFloat("_Specularity", config.Specularity);
		}

		private bool Better1911(FVRFireArm gun)
		{
			var obj = gun.ObjectWrapper;
			if (obj != null)
			{
				return obj.ItemID == _tacID || obj.ItemID == "M1911Operator";
			}

			return false;
		}

		private bool IsLowCapMag(FVRFireArmMagazine mag)
		{
			if (_configs.Magazine.FixMagPos.Value)
			{
				var obj = mag.ObjectWrapper;
				if (obj != null)
				{
					return mag.m_capacity <= 9;
				}
			}

			return false;
		}

		// Format the human readable RGBA to what unity wants
		private Vector4 Recolor(Vector4 cfg, float intensity)
		{
			var color = new Vector4(intensity * (cfg.x / 255), intensity * (cfg.y / 255), intensity * (cfg.z / 255), cfg.w / 1);
			return color;
		}

		#endregion Helpers
	}
}