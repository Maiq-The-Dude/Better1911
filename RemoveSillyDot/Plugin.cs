using Deli;
using FistVR;
using HarmonyLib;

namespace RemoveM1911TacRedDot
{
	public class Plugin : DeliBehaviour
	{
		[HarmonyPatch(typeof(HandgunSlide), "Awake")]
		[HarmonyPostfix]
		public static void SightRemove(HandgunSlide __instance)
		{
			var obj = __instance.Handgun.ObjectWrapper;
			if (obj != null)
			{
				if (obj.ItemID == "M1911Tactical")
				{
					var slideTF = __instance.transform;
					slideTF.Find("Sight").gameObject.SetActive(false);
				}
			}
		}

		void Awake()
		{
			Harmony.CreateAndPatchAll(typeof(Plugin));
		}
	}
}
