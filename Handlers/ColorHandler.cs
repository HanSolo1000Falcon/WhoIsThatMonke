using TMPro;
using UnityEngine;
using WhoIsTalking;
using WhoIsThatMonke.Classes;
using static WhoIsThatMonke.PublicVariablesGatherHere;

namespace WhoIsThatMonke.Handlers
{
    internal class ColorHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;
        GameObject fpTag, tpTag, firstPersonNameTag, thirdPersonNameTag;
        Renderer fpTextRenderer, fpColorRenderer, tpColorRenderer;
        TextMeshPro fpColorText, tpColorText;
        Shader uiShader = Shader.Find("UI/Default");
        private OffsetCalculatorCoolKidzOnly offsetCalculator;

        void Start()
        {
            if (firstPersonNameTag == null || thirdPersonNameTag == null)
            {
                CreateColorTags();
            }

            offsetCalculator = new OffsetCalculatorCoolKidzOnly();
            BoolChangedButOnlyTheGoodOnes += CalculateDaOffset;
        }

        void CalculateDaOffset()
        {
            offsetCalculator.ClearBoolsForDaSools();
            offsetCalculator.AddBool(isVelocityEnabled);
            offsetCalculator.AddBool(isFPSEnabled);
            float offset = offsetCalculator.CalculateOffsetCoolKidz();
            fpTag.transform.localPosition = new Vector3(0f, offset, 0f);
            tpTag.transform.localPosition = new Vector3(0f, offset, 0f);
        }

        private string GetColorCode(VRRig rig)
        {
            var color = rig.playerColor;
            int r, g, b;

            if (twoFiftyFiveColorCodes)
            {
                r = Mathf.RoundToInt(color.r * 255);
                g = Mathf.RoundToInt(color.g * 255);
                b = Mathf.RoundToInt(color.b * 255);
            }
            else
            {
                r = Mathf.RoundToInt(color.r * 9);
                g = Mathf.RoundToInt(color.g * 9);
                b = Mathf.RoundToInt(color.b * 9);
            }

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
                fpTag.transform.SetParent(firstPersonNameTag.transform);
                fpTag.transform.localPosition = new Vector3(0f, 4f, 0f);
                fpTag.transform.localScale = Vector3.one;
                fpTag.transform.localRotation = Quaternion.Euler(0, 180, 0);
                fpTag.layer = firstPersonNameTag.layer;

                Destroy(fpTag.GetComponent<Collider>());

                fpColorRenderer = fpTag.GetComponent<Renderer>();
                fpColorRenderer.material = new Material(uiShader);

                fpColorText = fpTag.AddComponent<TextMeshPro>();
                fpColorText.alignment = TextAlignmentOptions.Center;
                fpColorText.transform.rotation = Quaternion.Euler(0, 180, 0);
                fpColorText.font = nameTagHandler.rig.playerText1.font;
                fpColorText.fontSize = 7;
                fpColorText.text = GetColorCode(nameTagHandler.rig);
                fpColorText.color = nameTagHandler.rig.mainSkin.material.color;
            }

            if (thirdPersonNameTag == null)
            {
                Transform tmpchild1 = transform.FindChildRecursive("Third Person NameTag");
                thirdPersonNameTag = tmpchild1.FindChildRecursive("NameTag").gameObject;

                tpTag = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tpTag.name = "TP Color Holder";
                tpTag.transform.SetParent(thirdPersonNameTag.transform);
                tpTag.transform.localPosition = new Vector3(0f, 4f, 0f);
                tpTag.transform.localScale = Vector3.one;
                tpTag.transform.localRotation = Quaternion.Euler(0, 180, 0);
                tpTag.layer = thirdPersonNameTag.layer;

                Destroy(tpTag.GetComponent<Collider>());

                tpColorRenderer = tpTag.GetComponent<Renderer>();
                tpColorRenderer.material = new Material(uiShader);

                tpColorText = tpTag.AddComponent<TextMeshPro>();
                tpColorText.alignment = TextAlignmentOptions.Center;
                tpColorText.transform.rotation = Quaternion.Euler(0, 180, 0);
                tpColorText.font = nameTagHandler.rig.playerText1.font;
                tpColorText.fontSize = 7;
                tpColorText.text = GetColorCode(nameTagHandler.rig);
                tpColorText.color = nameTagHandler.rig.mainSkin.material.color;
            }

            UpdateColorPatchThingy();
        }

        void UpdateColorPatchThingy()
        {
            if (fpColorText != null)
            {
                fpColorText.text = GetColorCode(nameTagHandler.rig);
            }

            if (tpColorText != null)
            {
                tpColorText.text = GetColorCode(nameTagHandler.rig);
            }
        }

        void FixedUpdate()
        {
            if (nameTagHandler != null)
            {
                string currentColorCode = GetColorCode(nameTagHandler.rig);

                if (fpColorText.text != currentColorCode)
                {
                    fpColorText.text = currentColorCode;
                }

                if (tpColorText.text != currentColorCode)
                {
                    tpColorText.text = currentColorCode;
                }

                if (fpTextRenderer == null)
                {
                    fpTextRenderer = fpTag.transform.parent.GetComponent<Renderer>();
                }

                if (fpColorRenderer == null)
                {
                    fpColorRenderer = fpColorText.GetComponent<Renderer>();
                }

                if (tpColorRenderer == null)
                {
                    tpColorRenderer = tpColorText.GetComponent<Renderer>();
                }

                if (isColorCodeEnabled)
                {
                    tpColorRenderer.forceRenderingOff = false;
                    fpColorRenderer.forceRenderingOff = fpTextRenderer.forceRenderingOff;
                }
                else
                {
                    fpColorRenderer.forceRenderingOff = true;
                    tpColorRenderer.forceRenderingOff = true;
                }

                Color daRealColor = fpTextRenderer.material.color;

                fpColorText.color = daRealColor;
                tpColorText.color = daRealColor;

                if (nameTagHandler.rig.mainSkin.material.color != nameTagHandler.rig.playerColor)
                {
                    nameTagHandler.rig.mainSkin.material.color = nameTagHandler.rig.playerColor;
                }
            }
        }
    }
}