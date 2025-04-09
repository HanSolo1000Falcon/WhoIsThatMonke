using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using WhoIsTalking;
using System.Globalization;
using static WhoIsThatMonke.PublicVariablesGatherHere;

namespace WhoIsThatMonke.Handlers
{
    internal class FPSHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;
        public GameObject fpTag, tpTag, firstPersonNameTag, thirdPersonNameTag;
        public Renderer fpTextRenderer, fpFPSRenderer, tpFPSRenderer;
        public TextMeshPro fpFPSText, tpFPSText;
        public Shader uiShader = Shader.Find("UI/Default");

        void Start()
        {
            if (firstPersonNameTag == null || thirdPersonNameTag == null)
                CreateVelocityTags();
        }

        public void CreateVelocityTags()
        {
            if (firstPersonNameTag == null)
            {
                Transform tmpchild0 = transform.FindChildRecursive("First Person NameTag");
                firstPersonNameTag = tmpchild0.FindChildRecursive("NameTag").gameObject;

                fpTag = GameObject.CreatePrimitive(PrimitiveType.Quad);
                fpTag.name = "FP Velocity Holder";
                
                fpFPSRenderer, fpFPSText = TagInitHelp.InitTag(fpTag, firstPersonNameTag);
            }

            if (thirdPersonNameTag == null)
            {
                Transform tmpchild1 = transform.FindChildRecursive("Third Person NameTag");
                thirdPersonNameTag = tmpchild1.FindChildRecursive("NameTag").gameObject;

                tpTag = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tpTag.name = "TP Velocity Holder";
                
                tpFPSRenderer, tpFPSText = TagInitHelp.InitTag(tpTag, thirdPersonNameTag);
            }
            
            UpdateVelocityPatchThingy();
        }

        void UpdateVelocityPatchThingy()
        {
            if (fpFPSText != null)
            {
                fpFPSText.text = "0";
            }

            if (tpFPSText != null)
            {
                tpFPSText.text = "0";
            }
        }

        void FixedUpdate()
        {
            if (nameTagHandler != null)
            {
                fpFPSText.text = nameTagHandler.rig.fps.ToString() + " FPS";
                tpFPSText.text = nameTagHandler.rig.fps.ToString() + " FPS";

                if (fpTextRenderer == null)
                    fpTextRenderer = fpTag.transform.parent.GetComponent<Renderer>();

                if (fpFPSRenderer == null)
                    fpFPSRenderer = fpFPSText.GetComponent<Renderer>();

                if (tpFPSRenderer == null)
                    tpFPSRenderer = tpFPSText.GetComponent<Renderer>();

                tpFPSRenderer.forceRenderingOff = !isFPSEnabled;
                fpFPSRenderer.forceRenderingOff = !isFPSEnabled;

                if (!isVelocityEnabled)
                {
                    tpTag.transform.localPosition = new Vector3(0f, 2f, 0f);
                    fpTag.transform.localPosition = new Vector3(0f, 2f, 0f);
                }
                else
                {
                    tpTag.transform.localPosition = new Vector3(0f, 3f, 0f);
                    fpTag.transform.localPosition = new Vector3(0f, 3f, 0f);
                }

                fpFPSText.color = nameTagHandler.rig.playerColor;
                tpFPSText.color = nameTagHandler.rig.playerColor;
            }
        }
    }
}