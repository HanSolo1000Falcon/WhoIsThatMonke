using TMPro;
using UnityEngine;
using WhoIsTalking;
using static WhoIsThatMonke.PublicVariablesGatherHere;
using WhoIsThatMonke.Classes;

namespace WhoIsThatMonke.Handlers
{
    internal class FPSHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;
        GameObject fpTag, tpTag, firstPersonNameTag, thirdPersonNameTag;
        Renderer fpTextRenderer, fpFPSRenderer, tpFPSRenderer;
        TextMeshPro fpFPSText, tpFPSText;
        Shader uiShader = Shader.Find("UI/Default");
        private OffsetCalculatorCoolKidzOnly offsetCalculator;

        void Start()
        {
            if (firstPersonNameTag == null || thirdPersonNameTag == null)
            {
                CreateVelocityTags(); // cooli cool
            }
            offsetCalculator = new OffsetCalculatorCoolKidzOnly();
            BoolChangedButOnlyTheGoodOnes += CalculateDaOffset;
        }

        void CalculateDaOffset()
        {
            offsetCalculator.ClearBoolsForDaSools();
            offsetCalculator.AddBool(isVelocityEnabled);
            float offset = offsetCalculator.CalculateOffsetCoolKidz();
            fpTag.transform.localPosition = new Vector3(0f, offset, 0f);
            tpTag.transform.localPosition = new Vector3(0f, offset, 0f);
        }

        public void CreateVelocityTags()
        {
            if (firstPersonNameTag == null)
            {
                Transform tmpchild0 = transform.FindChildRecursive("First Person NameTag");
                firstPersonNameTag = tmpchild0.FindChildRecursive("NameTag").gameObject;

                fpTag = GameObject.CreatePrimitive(PrimitiveType.Quad);
                fpTag.name = "FP Velocity Holder";
                fpTag.transform.SetParent(firstPersonNameTag.transform);
                fpTag.transform.localPosition = new Vector3(0f, 3f, 0f);
                fpTag.transform.localScale = Vector3.one;
                fpTag.layer = firstPersonNameTag.layer;

                Destroy(fpTag.GetComponent<Collider>());

                fpFPSRenderer = fpTag.GetComponent<Renderer>();
                fpFPSRenderer.material = new Material(uiShader);

                fpFPSText = fpTag.AddComponent<TextMeshPro>();
                fpFPSText.alignment = TextAlignmentOptions.Center;
                fpFPSText.transform.rotation = Quaternion.Euler(0, 180, 0);
                fpFPSText.font = nameTagHandler.rig.playerText1.font;
                fpFPSText.fontSize = 7;
                fpFPSText.text = "0";
                fpFPSText.color = nameTagHandler.rig.playerColor;
            }

            if (thirdPersonNameTag == null)
            {
                Transform tmpchild1 = transform.FindChildRecursive("Third Person NameTag");
                thirdPersonNameTag = tmpchild1.FindChildRecursive("NameTag").gameObject;

                tpTag = GameObject.CreatePrimitive(PrimitiveType.Quad);
                tpTag.name = "TP Velocity Holder";
                tpTag.transform.SetParent(thirdPersonNameTag.transform);
                tpTag.transform.localPosition = new Vector3(0f, 3f, 0f);
                tpTag.transform.localScale = Vector3.one;
                tpTag.layer = thirdPersonNameTag.layer;

                Destroy(tpTag.GetComponent<Collider>());

                tpFPSRenderer = tpTag.GetComponent<Renderer>();
                tpFPSRenderer.material = new Material(uiShader);

                tpFPSText = tpTag.AddComponent<TextMeshPro>();
                tpFPSText.alignment = TextAlignmentOptions.Center;
                tpFPSText.transform.rotation = Quaternion.Euler(0, 180, 0);
                tpFPSText.font = nameTagHandler.rig.playerText1.font;
                tpFPSText.fontSize = 7;
                tpFPSText.text = "0";
                tpFPSText.color = nameTagHandler.rig.playerColor;
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
                {
                    fpTextRenderer = fpTag.transform.parent.GetComponent<Renderer>();
                }
                if (fpFPSRenderer == null)
                {
                    fpFPSRenderer = fpFPSText.GetComponent<Renderer>();
                }
                if (tpFPSRenderer == null)
                {
                    tpFPSRenderer = tpFPSText.GetComponent<Renderer>();
                }
                if (isFPSEnabled)
                {
                    tpFPSRenderer.forceRenderingOff = false;
                    fpFPSRenderer.forceRenderingOff = fpTextRenderer.forceRenderingOff;
                }
                else
                {
                    fpFPSRenderer.forceRenderingOff = true;
                    tpFPSRenderer.forceRenderingOff = true;
                }

                Color daRealColor = fpTextRenderer.material.color;

                fpFPSText.color = daRealColor;
                tpFPSText.color = daRealColor;
            }
        }
    }
}