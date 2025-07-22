using HarmonyLib;
using WhoIsTalking;
using WhoIsThatMonke.Handlers;
namespace WhoIsThatMonke.Patches
{
    [HarmonyPatch(typeof(NameTagHandler), "RefreshInfo")]
    public class WhoIsTalkingPatch0
    {
        private static void Postfix(NameTagHandler __instance)
        {
            __instance.GetOrAddComponent<PlatformHandler>(out var NTH);
            NTH.nameTagHandler = __instance;
            __instance.GetOrAddComponent<ColorHandler>(out var NTH2);
            NTH2.nameTagHandler = __instance;
            __instance.GetOrAddComponent<VelocityHandler>(out var NTH3);
            NTH3.nameTagHandler = __instance;
            __instance.GetOrAddComponent<FPSHandler>(out var NTH4);
            NTH4.nameTagHandler = __instance;
        }
    }
}