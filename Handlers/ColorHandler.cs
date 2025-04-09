using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using WhoIsTalking;
using static WhoIsThatMonke.PublicVariablesGatherHere;

namespace WhoIsThatMonke.Handlers
{
    internal class ColorHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;
        public GameObject fpTag, tpTag, firstPersonNameTag, thirdPersonNameTag;
        public Renderer fpTextRenderer, fpColorRenderer, tpColorRenderer;
        public TextMeshPro fpColorText, tpColorText;
        public Shader uiShader = Shader.Find("UI/Default");

        void Start()
        {
            if (firstPersonNameTag == null || thirdPersonNameTag == null)
                CreateColorTags();
        }

        private string GetColorCode(VRRig rig)
        {
            var color = rig.playerColor;
            int r = Mathf.RoundToInt(color.r * 9);
            int g = Mathf.RoundToInt(color.g * 9);
            int b = Mathf.RoundToInt(color.b * 9);
            return $"{r}, {g}, {b}";
        }

        public void CreateColorTags()
        {
            if (firstPersonNameTag == null)
            {
                Transform tmpchild0 = transform.FindChildRecursive("First Person NameTag");
                firstPersonNameTag = tmpchild0.FindChildRecursive("NameTag").gameObject;

                fpTag = GameObject.CreatePrimitive(PrimitiveType.Quad);
                fpTag.name = "FP Color Holder";
                
                fpColorRenderer, fpColorText = TagManager.InitTag(fpTag, firstPersonNameTag);
            }

            if (thirdPersonNameTag == null)
            {
                Transform tmpchild1 = transform.FindChildRecursive("Third Person NameTag");
                thirdPersonNameTag = tmpchild1.FindChildRecursive("NameTag").gameObject;

                tpTag = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tpTag.name = "TP Color Holder";
                
                tpColorRenderer, tpColorText = TagManager.InitTag(tpTag, thirdPersonNameTag);
            }

            UpdateColorPatchThingy();
        }

        void UpdateColorPatchThingy()
        {
            if (fpColorText != null)
                fpColorText.text = GetColorCode(nameTagHandler.rig);

            if (tpColorText != null)
                tpColorText.text = GetColorCode(nameTagHandler.rig);
        }

        private void UpdateTagLocalPosition(GameObject tag1, GameObject tag2, Vector3 newV) {
            tag1.transform.localPosition = newV;
            tag1.transform.localPosition = newV;
        }

        void FixedUpdate()
        {
            if (nameTagHandler != null)
            {
                if (fpColorText.text != GetColorCode(nameTagHandler.rig))
                    fpColorText.text = GetColorCode(nameTagHandler.rig);
                
                if (tpColorText.text != GetColorCode(nameTagHandler.rig))
                    tpColorText.text = GetColorCode(nameTagHandler.rig);

                if (fpTextRenderer == null)
                    fpTextRenderer = fpTag.transform.parent.GetComponent<Renderer>();

                if (fpColorRenderer == null)
                    fpColorRenderer = fpColorText.GetComponent<Renderer>();
                
                if (tpColorRenderer == null)
                    tpColorRenderer = tpColorText.GetComponent<Renderer>();

                tpColorRenderer.forceRenderingOff = !isColorCodeEnabled;
                fpColorRenderer.forceRenderingOff = !isColorCodeEnabled;

                if (!isVelocityEnabled && !isFPSEnabled)
                    UpdateTagLocalPosition(fpTag, tpTag, new Vector3(0f, 2f, 0f));             
                else if (!isFPSEnabled)
                    UpdateTagLocalPosition(fpTag, tpTag, new Vector3(0f, 3f, 0f));
                else if (!isVelocityEnabled)
                    UpdateTagLocalPosition(fpTag, tpTag, new Vector3(0f, 3f, 0f));
                else
                    UpdateTagLocalPosition(fpTag, tpTag, new Vector3(0f, 4f, 0f));

                fpColorText.color = nameTagHandler.rig.playerColor;
                tpColorText.color = nameTagHandler.rig.playerColor;

                if (nameTagHandler.rig.mainSkin.material.color != nameTagHandler.rig.playerColor)
                {
                    nameTagHandler.rig.mainSkin.material.color = nameTagHandler.rig.playerColor;
                }
            }
        }
    }
}