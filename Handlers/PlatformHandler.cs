using PlayFab.ClientModels;
using PlayFab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using WhoIsTalking;
using static WhoIsThatMonke.PublicVariablesGatherHere;

namespace WhoIsThatMonke.Handlers
{
    internal class PlatformHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;
        public Texture2D pcTexture, steamTexture, standaloneTexture, dasMeTexture, dasGrazeTexture, dasbaggZTexture, dasMonkyTexture, notSureTexture;
        public GameObject fpPlatformIcon, tpPlatformIcon, firstPersonNameTag, thirdPersonNameTag;
        public Renderer fpPlatformRenderer, tpPlatformRenderer, fpTextRenderer;
        public Shader UIShader = Shader.Find("UI/Default");
        public DateTime whenWasGorillaTagPaidOrSmthIDKOculus = new DateTime(2023, 02, 06), createdDate;
        private string lastName, myUserID = "A48744B93D9A3596", grazeUserID = "42D7D32651E93866", baggZuserID = "9ABD0C174289F58E", monkyUserID = "B1B20DEEEDB71C63";
        // maybe dont store them like this and just pull them from a JSON or something ^^^^

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
            CreatePlatformIcons();
        }

        private void GetAccountCreationDate(string userID, Action<GetAccountInfoResult> result)
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
            {
                PlayFabId = userID
            }, result, error =>
            {
                Debug.LogError("Failed to get account info: " + error.ErrorMessage);
            });
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

        //this just makes it more readable -Graze
        Texture GetPlatformTexture(string concat)
        {
            GetAccountCreationDate(nameTagHandler.player.UserId, result =>
            {
                createdDate = result.AccountInfo.Created;
            });
            if (nameTagHandler.player.UserId == myUserID)
            {
                return dasMeTexture;
            }
            else if (nameTagHandler.player.UserId == grazeUserID)
            {
                return dasGrazeTexture;
            }
            else if (nameTagHandler.player.UserId == baggZuserID)
            {
                return dasbaggZTexture;
            }
            else if (nameTagHandler.player.UserId == monkyUserID)
            {
                return dasMonkyTexture;
            }
            else if (concat.Contains("S. FIRST LOGIN"))
            {
                return steamTexture;
            }
            else if (concat.Contains("FIRST LOGIN") || nameTagHandler.rig.OwningNetPlayer.GetPlayerRef().CustomProperties.Count() >= 2)
            {
                return pcTexture;
            }
            else if (concat.Contains("LMAKT."))
            {
                return standaloneTexture;
            }
            else if (createdDate > whenWasGorillaTagPaidOrSmthIDKOculus)
            {
                return standaloneTexture;
            }
            else
            {
                return notSureTexture;
            }
        }

        //this just makes it more readable -Graze
        public void UpdatePlatformPatchThingy()
        {
            if (fpPlatformRenderer != null)
            {
                fpPlatformRenderer.material.mainTexture = GetPlatformTexture(nameTagHandler.rig.concatStringOfCosmeticsAllowed);
            }
            if (tpPlatformRenderer != null)
            {
                tpPlatformRenderer.material.mainTexture = GetPlatformTexture(nameTagHandler.rig.concatStringOfCosmeticsAllowed);
            }
        }

        private void ChangePositionOfTheThingy()
        {
            switch (nameTagHandler.player.NickName.Length)
            {
                case 1:
                    fpPlatformIcon.transform.localPosition = new Vector3(-0.75f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-0.75f, 0f, 0f);
                    break;

                case 2:
                    fpPlatformIcon.transform.localPosition = new Vector3(-1f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-1f, 0f, 0f);
                    break;

                case 3:
                    fpPlatformIcon.transform.localPosition = new Vector3(-1.25f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-1.25f, 0f, 0f);
                    break;

                case 4:
                    fpPlatformIcon.transform.localPosition = new Vector3(-1.5f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-1.5f, 0f, 0f);
                    break;

                case 5:
                    fpPlatformIcon.transform.localPosition = new Vector3(-1.75f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-1.75f, 0f, 0f);
                    break;

                case 6:
                    fpPlatformIcon.transform.localPosition = new Vector3(-2f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-2f, 0f, 0f);
                    break;

                case 7:
                    fpPlatformIcon.transform.localPosition = new Vector3(-2.25f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-2.25f, 0f, 0f);
                    break;

                case 8:
                    fpPlatformIcon.transform.localPosition = new Vector3(-2.5f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-2.5f, 0f, 0f);
                    break;

                case 9:
                    fpPlatformIcon.transform.localPosition = new Vector3(-2.75f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-2.75f, 0f, 0f);
                    break;

                case 10:
                    fpPlatformIcon.transform.localPosition = new Vector3(-3f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-3f, 0f, 0f);
                    break;

                case 11:
                    fpPlatformIcon.transform.localPosition = new Vector3(-3.25f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-3.25f, 0f, 0f);
                    break;

                case 12:
                    fpPlatformIcon.transform.localPosition = new Vector3(-3.5f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-3.5f, 0f, 0f);
                    break;

                case 13:
                    fpPlatformIcon.transform.localPosition = new Vector3(-3.75f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-3.75f, 0f, 0f);
                    break;

                case 14:
                    fpPlatformIcon.transform.localPosition = new Vector3(-4f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-4f, 0f, 0f);
                    break;

                case 15:
                    fpPlatformIcon.transform.localPosition = new Vector3(-4.25f, 0f, 0f);
                    tpPlatformIcon.transform.localPosition = new Vector3(-4.25f, 0f, 0f);
                    break;
            }
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
