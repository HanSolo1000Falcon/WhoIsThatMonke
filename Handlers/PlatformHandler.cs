using PlayFab.ClientModels;
using PlayFab;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using WhoIsTalking;
using static WhoIsThatMonke.PublicVariablesGatherHere;
using System.Threading.Tasks;

namespace WhoIsThatMonke.Handlers
{
    public class PlatformHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;

        private Texture2D pcTexture, steamTexture, standaloneTexture, notSureTexture;
        private Texture2D dasMeTexture, dasGrazeTexture, dasbaggZTexture, dasMonkyTexture, dasArielTexture;

        private GameObject fpPlatformIcon, tpPlatformIcon;
        private GameObject firstPersonNameTag, thirdPersonNameTag;

        private Renderer fpPlatformRenderer, tpPlatformRenderer;
        private Renderer fpTextRenderer, tpTextRenderer;

        private Shader UIShader;

        private readonly DateTime oculusThresholdDate = new DateTime(2023, 02, 06);
        private string lastName;

        private readonly Dictionary<string, Texture2D> knownUserTextures = new();

        private const string myUserID = "A48744B93D9A3596", grazeUserID = "42D7D32651E93866", 
                             baggZuserID = "9ABD0C174289F58E", monkyUserID = "B1B20DEEEDB71C63", 
                             arielUserID = "C41A1A9055417A27";

        private void Awake() => UIShader = Shader.Find("UI/Default");

        private void Start()
        {
            InitializeTextures();
            CreatePlatformIcons();
        }

        private void InitializeTextures()
        {
            pcTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.PCIcon.png");
            steamTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.SteamIcon.png");
            standaloneTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.MetaIcon.png");
            notSureTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.Questionmark.png");

            dasMeTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.ProfilbildGTAG.png");
            dasGrazeTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.GrazeIcon.png");
            dasbaggZTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.BaggZIcon.png");
            dasMonkyTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.MonkyIcon.png");
            dasArielTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.ArielIcon.png");

            knownUserTextures.Add(myUserID, dasMeTexture);
            knownUserTextures.Add(grazeUserID, dasGrazeTexture);
            knownUserTextures.Add(baggZuserID, dasbaggZTexture);
            knownUserTextures.Add(monkyUserID, dasMonkyTexture);
            knownUserTextures.Add(arielUserID, dasArielTexture);
        }

        private Texture2D LoadEmbeddedImage(string resourcePath)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                Debug.LogError($"Resource '{resourcePath}' not found.");
                return null;
            }

            byte[] imageData = new byte[stream.Length];
            stream.Read(imageData, 0, imageData.Length);

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            return texture;
        }

        private void CreatePlatformIcons()
        {
            if (firstPersonNameTag == null)
            {
                firstPersonNameTag = transform.FindChildRecursive("First Person NameTag")?.FindChildRecursive("NameTag")?.gameObject;
                if (firstPersonNameTag != null)
                    CreateIcon(ref fpPlatformIcon, ref fpPlatformRenderer, firstPersonNameTag);
            }

            if (thirdPersonNameTag == null)
            {
                thirdPersonNameTag = transform.FindChildRecursive("Third Person NameTag")?.FindChildRecursive("NameTag")?.gameObject;
                if (thirdPersonNameTag != null)
                    CreateIcon(ref tpPlatformIcon, ref tpPlatformRenderer, thirdPersonNameTag);
            }

            UpdatePlatformPatchThingy();
        }

        private void CreateIcon(ref GameObject iconObj, ref Renderer renderer, GameObject parent)
        {
            iconObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            iconObj.name = $"{(parent == firstPersonNameTag ? "FP" : "TP")} Platform Icon";
            iconObj.transform.SetParent(parent.transform);
            iconObj.transform.localPosition = Vector3.zero;
            iconObj.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            iconObj.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            iconObj.layer = parent.layer;

            Destroy(iconObj.GetComponent<Collider>());

            renderer = iconObj.GetComponent<Renderer>();
            renderer.material = new Material(UIShader);
        }

        private async Task<GetAccountInfoResult> GetAccountCreationDateAsync(string userID)
        {
            var tcs = new TaskCompletionSource<GetAccountInfoResult>();

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { PlayFabId = userID },
                result => tcs.SetResult(result),
                error =>
                {
                    Debug.LogError("Failed to get account info: " + error.ErrorMessage);
                    tcs.SetException(new Exception(error.ErrorMessage));
                });

            return await tcs.Task;
        }

        private async Task<Texture> GetPlatformTextureAsync(string concat)
        {
            string userId = nameTagHandler.player.UserId;
            int propCount = nameTagHandler.player.GetPlayerRef().CustomProperties.Count;

            if (knownUserTextures.TryGetValue(userId, out Texture2D known)) return known;
            if (concat.Contains("S. FIRST LOGIN")) return steamTexture;
            if (concat.Contains("FIRST LOGIN") || propCount > 1) return pcTexture;
            if (concat.Contains("LMAKT.")) return standaloneTexture;

            try
            {
                var accountInfo = await GetAccountCreationDateAsync(userId);
                return accountInfo.AccountInfo.Created > oculusThresholdDate ? standaloneTexture : notSureTexture;
            }
            catch
            {
                return notSureTexture;
            }
        }

        public async void UpdatePlatformPatchThingy()
        {
            Texture platformTexture = await GetPlatformTextureAsync(nameTagHandler.rig.concatStringOfCosmeticsAllowed);

            if (fpPlatformRenderer != null)
                fpPlatformRenderer.material.mainTexture = platformTexture;

            if (tpPlatformRenderer != null)
                tpPlatformRenderer.material.mainTexture = platformTexture;
        }

        private void ChangePositionOfTheThingy()
        {
            string name = nameTagHandler.player.NickName;
            float offset = string.IsNullOrEmpty(name) ? 0f : -(name.Length * 0.25f + 0.5f);

            if (fpPlatformIcon != null)
                fpPlatformIcon.transform.localPosition = new Vector3(offset, 0f, 0f);

            if (tpPlatformIcon != null)
                tpPlatformIcon.transform.localPosition = new Vector3(offset, 0f, 0f);
        }

        private void FixedUpdate()
        {
            if (fpPlatformIcon == null || nameTagHandler == null)
                return;

            fpTextRenderer ??= fpPlatformIcon.transform.parent.GetComponent<Renderer>();
            tpTextRenderer ??= tpPlatformIcon?.transform.parent.GetComponent<Renderer>();
            fpPlatformRenderer ??= fpPlatformIcon.GetComponent<Renderer>();
            tpPlatformRenderer ??= tpPlatformIcon?.GetComponent<Renderer>();

            bool shouldRender = isPlatformEnabled && fpTextRenderer != null && tpTextRenderer != null;

            if (fpPlatformRenderer != null)
                fpPlatformRenderer.forceRenderingOff = !shouldRender || fpTextRenderer.forceRenderingOff;

            if (tpPlatformRenderer != null)
                tpPlatformRenderer.forceRenderingOff = !shouldRender || tpTextRenderer.forceRenderingOff;

            if (lastName != nameTagHandler.player.NickName)
            {
                lastName = nameTagHandler.player.NickName;
                ChangePositionOfTheThingy();
            }
        }
    }
}