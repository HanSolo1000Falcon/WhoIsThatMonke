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
    internal class PlatformHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;
        Texture2D pcTexture, steamTexture, standaloneTexture, dasMeTexture, dasGrazeTexture, dasbaggZTexture, dasMonkyTexture, notSureTexture;
        GameObject fpPlatformIcon, tpPlatformIcon, firstPersonNameTag, thirdPersonNameTag;
        Renderer fpPlatformRenderer, tpPlatformRenderer, fpTextRenderer;
        Shader UIShader = Shader.Find("UI/Default");
        DateTime whenWasGorillaTagPaidOrSmthIDKOculus = new DateTime(2023, 02, 06), createdDate;
        string lastName;
        const string myUserID = "A48744B93D9A3596", grazeUserID = "42D7D32651E93866", baggZuserID = "9ABD0C174289F58E", monkyUserID = "B1B20DEEEDB71C63";
        Dictionary<string, Texture2D> knownUserTextures;


        void Start()
        {
            pcTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.PCIcon.png");
            steamTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.SteamIcon.png");
            standaloneTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.MetaIcon.png");
            dasMeTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.ProfilbildGTAG.png");
            dasGrazeTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.GrazeIcon.png");
            dasbaggZTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.BaggZIcon.png");
            dasMonkyTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.MonkyIcon.png");
            notSureTexture = LoadEmbeddedImage("WhoIsThatMonke.Assets.Questionmark.png");

            knownUserTextures = new Dictionary<string, Texture2D>()
            {
                { myUserID, dasMeTexture },
                { grazeUserID, dasGrazeTexture },
                { baggZuserID, dasbaggZTexture },
                { monkyUserID, dasMonkyTexture },
            };

            CreatePlatformIcons();
        }

        async Task<GetAccountInfoResult> GetAccountCreationDateAsync(string userID)
        {
            var tcs = new TaskCompletionSource<GetAccountInfoResult>();

            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
            {
                PlayFabId = userID
            },
            result => tcs.SetResult(result),
            error =>
            {
                Debug.LogError("Failed to get account info: " + error.ErrorMessage);
                tcs.SetException(new Exception(error.ErrorMessage));
            });

            return await tcs.Task;
        }

        private Texture2D LoadEmbeddedImage(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
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
        }

        public void CreatePlatformIcons()
        {
            //This is a more officent way to do it than a foreach as i know the child names -Graze
            if (firstPersonNameTag == null)
            {
                Transform tmpchild0 = transform.FindChildRecursive("First Person NameTag");
                firstPersonNameTag = tmpchild0.FindChildRecursive("NameTag").gameObject;

                fpPlatformIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
                fpPlatformIcon.name = "FP Platform Icon";
                fpPlatformIcon.transform.SetParent(firstPersonNameTag.transform);
                fpPlatformIcon.transform.localPosition = new Vector3(-3f, 0f, 0f);
                fpPlatformIcon.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                fpPlatformIcon.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                fpPlatformIcon.layer = firstPersonNameTag.layer;

                Destroy(fpPlatformIcon.GetComponent<Collider>());

                fpPlatformRenderer = fpPlatformIcon.GetComponent<Renderer>();
                fpPlatformRenderer.material = new Material(UIShader);
            }

            if (thirdPersonNameTag == null)
            {
                Transform tmpchild1 = transform.FindChildRecursive("Third Person NameTag");
                thirdPersonNameTag = tmpchild1.FindChildRecursive("NameTag").gameObject;

                tpPlatformIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tpPlatformIcon.name = "TP Platform Icon";
                tpPlatformIcon.transform.SetParent(thirdPersonNameTag.transform);
                tpPlatformIcon.transform.localPosition = new Vector3(-3.25f, 0f, 0f);
                tpPlatformIcon.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                tpPlatformIcon.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                tpPlatformIcon.layer = thirdPersonNameTag.layer;

                Destroy(tpPlatformIcon.GetComponent<Collider>());

                tpPlatformRenderer = tpPlatformIcon.GetComponent<Renderer>();
                tpPlatformRenderer.material = new Material(UIShader);
            }
            UpdatePlatformPatchThingy();
        }

        async Task<Texture> GetPlatformTextureAsync(string concat)
        {
            string userId = nameTagHandler.player.UserId;
            int customPropsCount = nameTagHandler.player.GetPlayerRef().CustomProperties.Count;

            if (knownUserTextures.TryGetValue(userId, out Texture2D knownTexture)) return knownTexture;
            if (concat.Contains("S. FIRST LOGIN")) return steamTexture;
            if (concat.Contains("FIRST LOGIN") || customPropsCount >= 2) return pcTexture;
            if (concat.Contains("LMAKT.")) return standaloneTexture;

            var accountInfo = await GetAccountCreationDateAsync(userId); 
            createdDate = accountInfo.AccountInfo.Created;

            if (createdDate > whenWasGorillaTagPaidOrSmthIDKOculus) return standaloneTexture;
            return notSureTexture;
        }

        public async void UpdatePlatformPatchThingy()
        {
            Texture platformTexture = await GetPlatformTextureAsync(nameTagHandler.rig.concatStringOfCosmeticsAllowed);

            if (fpPlatformRenderer != null)
            {
                fpPlatformRenderer.material.mainTexture = platformTexture;
            }

            if (tpPlatformRenderer != null)
            {
                tpPlatformRenderer.material.mainTexture = platformTexture;
            }
        }

        private float ChangePositionOfTheThingy()
        {
            float offset = nameTagHandler.player.NickName.Length * 0.25f;
            if (nameTagHandler.player.NickName.Length == 0)
            {
                return 0f;
            }
            return -(offset + 0.5f);
        }

        //Only the First person One is hidden so i changed this to do that
        //no longer have to manualy set rotation etc as it is parented to the nametag Text Obj -Graze
        void FixedUpdate()
        {
            if (fpPlatformIcon != null)
            {
                if (fpTextRenderer == null)
                {
                    fpTextRenderer = fpPlatformIcon.transform.parent.GetComponent<Renderer>();
                }
                if (fpPlatformRenderer == null)
                {
                    fpPlatformRenderer = fpPlatformIcon.GetComponent<Renderer>();
                }
                if (tpPlatformRenderer == null)
                {
                    tpPlatformRenderer = tpPlatformIcon.GetComponent<Renderer>();
                }

                if (isPlatformEnabled)
                {
                    tpPlatformRenderer.forceRenderingOff = false;
                    fpPlatformRenderer.forceRenderingOff = fpTextRenderer.forceRenderingOff;
                }
                else
                {
                    fpPlatformRenderer.forceRenderingOff = true;
                    tpPlatformRenderer.forceRenderingOff = true;
                }

                if (lastName != nameTagHandler.player.NickName)
                {
                    lastName = nameTagHandler.player.NickName;
                    ChangePositionOfTheThingy();
                }
            }
        }
    }
}
