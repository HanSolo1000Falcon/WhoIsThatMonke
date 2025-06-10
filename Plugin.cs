using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Photon.Pun;
using static WhoIsThatMonke.PublicVariablesGatherHere;

namespace WhoIsThatMonke
{
    // This is your mod's main class.
    [BepInIncompatibility("com.hansolo1000falcon.gorillatag.whatisthefps")]
    [BepInIncompatibility("com.hansolo1000falcon.gorillatag.whoischeating")]
    [BepInIncompatibility("com.hansolo1000falcon.gorillatag.whoisspeeding")]
    [BepInIncompatibility("org.iidk.gorillatag.iimenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public ConfigFile cfg;
        public ConfigEntry<bool> PlatformCheckerEnabled;
        public ConfigEntry<bool> VelocityCheckerEnabled;
        public ConfigEntry<bool> FPSCheckerEnabled;
        public ConfigEntry<bool> ColorCodeSpooferEnabled;
        public ConfigEntry<bool> TwoFiveFiveColorCodesEnabled;
        public static Plugin Instance;

        void Start()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("cheese is gouda", PluginInfo.Name);
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }

 	    void Awake()
  	    {
            Instance = this;
            var cfgPath = Path.Combine(Paths.ConfigPath, "WhoIsThatMonke.cfg" + " " + PluginInfo.Version);
  	        cfg = new ConfigFile(cfgPath, true);
            PlatformCheckerEnabled = cfg.Bind("Settings", "Platform Checker", true, "Enable or disable the platform checker.");
            VelocityCheckerEnabled = cfg.Bind("Settings", "Velocity Checker", true, "Enable or disable the velocity checker.");
            FPSCheckerEnabled = cfg.Bind("Settings", "FPS Checker", true, "Enable or disable the FPS checker.");
            ColorCodeSpooferEnabled = cfg.Bind("Settings", "Color Code Spoofer", true, "Enable or disable the color code spoofer.");
            TwoFiveFiveColorCodesEnabled = cfg.Bind("Settings", "255 Color Codes", false, "Enable or disable 255 color codes.");
            

            isPlatformEnabled = PlatformCheckerEnabled.Value;
            isVelocityEnabled = VelocityCheckerEnabled.Value;
            isFPSEnabled = FPSCheckerEnabled.Value;
            isColorCodeEnabled = ColorCodeSpooferEnabled.Value;
        }
    }
}
