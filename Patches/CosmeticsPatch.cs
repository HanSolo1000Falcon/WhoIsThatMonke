using HarmonyLib;
using WhoIsThatMonke.Handlers;

namespace WhoIsThatMonke.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("IUserCosmeticsCallback.OnGetUserCosmetics", MethodType.Normal)]
    public class CosmeticsPatch
    {
        private static void Postfix(VRRig __instance) => __instance.GetComponent<PlatformHandler>().UpdatePlatformPatchThingy();
    }
}