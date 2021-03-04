using Deli.Setup;
using HarmonyLib;
using Better1911.Configs;

namespace Better1911
{
	public class Plugin : DeliBehaviour
	{
		public static Plugin Instance { get; private set; }

		public RootConfig Configs { get; }

		private readonly Patches _patches;

		public Plugin()
		{
			Instance = this;

			Configs = new RootConfig(Config);
			_patches = new Patches(Config);
		}

		private void Awake()
		{
			Harmony.CreateAndPatchAll(typeof(Patches));
		}
	}
}
