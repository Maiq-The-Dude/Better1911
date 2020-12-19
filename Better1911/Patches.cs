using FistVR;
using HarmonyLib;
using Better1911.Configs;
using UnityEngine;
using System;
using BepInEx.Configuration;

namespace Better1911
{
	internal class Patches
	{
		private static RootConfig Config => Plugin.Instance.Configs;
		private static ConfigFile _cfg;

		private const string _tacMagID = "MagazineM1911Tactical";
		private const string _dilMagID = "MagazineM1911Dillinger";

		public Patches(ConfigFile config)
		{
			_cfg = config;
		}

		#region M1911Tactical Customization

		[HarmonyPatch(typeof(HandgunSlide), "Awake")]
		[HarmonyPostfix]
		private static void ChangeHandgun(HandgunSlide __instance)
		{
			var obj = __instance.Handgun.ObjectWrapper;
			if (obj != null)
			{
				// M1911Operator is a lost cause
				if (obj.ItemID == "M1911Tactical")
				{
					ReloadConfig(_cfg);
					var slideTF = __instance.transform;

					// Red Dot
					if (Config.DisableRedDot.Value)
					{
						slideTF.Find("Sight").gameObject.SetActive(false);
					}

					// Glow Sights
					var glowCfg = Config.GlowSights;
					if (glowCfg.CustomColor.Value)
					{
						slideTF.Find("GlowSight (1)").GetComponent<Renderer>().material.SetColor("_Color", glowCfg.RearColor.Value);
						slideTF.Find("GlowSight (2)").GetComponent<Renderer>().material.SetColor("_Color", glowCfg.RearColor.Value);
						slideTF.Find("GlowSight (3)").GetComponent<Renderer>().material.SetColor("_Color", glowCfg.FrontColor.Value);
					}
			
					// Gun Materials
					var gunCfg = Config.GunCustomization;
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
		}

		#endregion

		#region Magazine Fix and Customization

		[HarmonyPatch(typeof(FVRFireArmMagazine), "Load")]
		[HarmonyPostfix]
		private static void Patch(FVRFireArmMagazine __instance)
		{
			if (Better1911(__instance.FireArm))
			{
				ReloadConfig(_cfg);
				var tf = __instance.transform;
				var obj = __instance.ObjectWrapper;

				// Magic numbers to fix low cap mag pose/scale
				if (FixPose(__instance))
				{
					tf.localScale = new Vector3(1f, 1.07f, 1f);
					tf.localPosition += new Vector3(0, -0.0052f, 0);
				}

				// Magazine Materials
				if (__instance.FireArm.ObjectWrapper.ItemID == "M1911Tactical")
				{
					var cfg = Config.Magazine;
					if (cfg.CustomMagazineMaterial.Value != MagazineConfig.MagazineMaterial.Default)
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
											PaintMag(mat, Config.GunCustomization.Frame, obj.ItemID);
											break;
										case (MagazineConfig.MagazineMaterial.Slide):
											PaintMag(mat, Config.GunCustomization.Slide, obj.ItemID);
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

		[HarmonyPatch(typeof(FVRFireArmMagazine), "Release")]
		[HarmonyPrefix]
		private static bool Unpatch(FVRFireArmMagazine __instance)
		{
			if (Better1911(__instance.FireArm) && FixPose(__instance))
			{
				__instance.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			return true;
		}

		#endregion

		#region Helpers

		private static void ReloadConfig(ConfigFile config)
		{
			config.Reload();
		}

		private static void PaintMag(Material mat, IConfig config, string id)
		{
			if (id == _tacMagID)
			{
				mat.SetFloat("_Metal", config.Metallic);
				mat.SetFloat("_BumpScale", config.NormalStrength);
				mat.SetColor("_Color", config.Recolor);
				mat.SetFloat("_Specularity", config.Specularity);
			}
			else
			{
				if (Config.GunCustomization.Frame.RemoveTexture.Value)
				{
					mat.SetTexture("_MainTex", null);
				}
				PaintComponent(mat, config);
			}	
		}

		private static void PaintComponent(Material mat, IConfig config)
		{
			mat.SetFloat("_Metal", config.Metallic);
			mat.SetFloat("_BumpScale", config.NormalStrength);
			mat.SetFloat("_Roughness", config.Roughness);
			mat.SetColor("_Color", config.Recolor);
			mat.SetFloat("_Specularity", config.Specularity);
		}

		private static bool Better1911(FVRFireArm gun)
		{
			var obj = gun.ObjectWrapper;
			if (obj != null)
			{
				if (obj.ItemID == "M1911Tactical" || obj.ItemID == "M1911Operator")
				{
					return true;
				}
			}
			return false;
		}

		private static bool FixPose(FVRFireArmMagazine mag)
		{
			if(Config.Magazine.FixMagPos.Value)
			{
				var obj = mag.ObjectWrapper;
				if (obj != null)
				{
					if (mag.m_capacity <= 9)
					{
						return true;
					}
				}
			}
			
			return false;
		}

		#endregion
	}
}
