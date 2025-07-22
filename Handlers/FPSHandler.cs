using TMPro;
using UnityEngine;
using WhoIsTalking;
using static WhoIsThatMonke.PublicVariablesGatherHere;
using WhoIsThatMonke.Classes;

namespace WhoIsThatMonke.Handlers
{
    public class FPSHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;

        private GameObject fpTag, tpTag;
        private GameObject firstPersonNameTag, thirdPersonNameTag;
        private TextMeshPro fpFPSText, tpFPSText;
        private Renderer fpFPSRenderer, tpFPSRenderer;
        private Renderer fpTextRenderer, tpTextRenderer;

        private Shader uiShader;
        private OffsetCalculatorCoolKidzOnly offsetCalculator = new OffsetCalculatorCoolKidzOnly();

        private void Awake() => uiShader = Shader.Find("UI/Default");

        private void Start()
        {
            CreateVelocityTagsIfNeeded();
            BoolChangedButOnlyTheGoodOnes += CalculateDaOffset;
        }

        private void CreateVelocityTagsIfNeeded()
        {
            if (firstPersonNameTag == null)
            {
                firstPersonNameTag = transform.FindChildRecursive("First Person NameTag")?.FindChildRecursive("NameTag")?.gameObject;
                if (firstPersonNameTag != null)
                    CreateVelocityTag(ref fpTag, ref fpFPSText, ref fpFPSRenderer, firstPersonNameTag);
            }

            if (thirdPersonNameTag == null)
            {
                thirdPersonNameTag = transform.FindChildRecursive("Third Person NameTag")?.FindChildRecursive("NameTag")?.gameObject;
                if (thirdPersonNameTag != null)
                    CreateVelocityTag(ref tpTag, ref tpFPSText, ref tpFPSRenderer, thirdPersonNameTag);
            }

            UpdateFPSTexts("0");
        }

        private void CreateVelocityTag(ref GameObject tagObj, ref TextMeshPro textObj, ref Renderer rendererObj, GameObject parent)
        {
            tagObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            tagObj.name = $"{(parent == firstPersonNameTag ? "FP" : "TP")} Velocity Holder";
            tagObj.transform.SetParent(parent.transform);
            tagObj.transform.localPosition = new Vector3(0f, 3f, 0f);
            tagObj.transform.localScale = Vector3.one;
            tagObj.layer = parent.layer;

            Destroy(tagObj.GetComponent<Collider>());

            rendererObj = tagObj.GetComponent<Renderer>();
            rendererObj.material = new Material(uiShader);

            textObj = tagObj.AddComponent<TextMeshPro>();
            textObj.alignment = TextAlignmentOptions.Center;
            textObj.transform.rotation = Quaternion.Euler(0, 180, 0);
            textObj.font = nameTagHandler.rig.playerText1.font;
            textObj.fontSize = 7;
            textObj.color = nameTagHandler.rig.playerColor;
        }

        private void CalculateDaOffset()
        {
            offsetCalculator.ClearBoolsForDaSools();
            offsetCalculator.AddBool(isVelocityEnabled);
            float offset = offsetCalculator.CalculateOffsetCoolKidz();

            if (fpTag) fpTag.transform.localPosition = new Vector3(0f, offset, 0f);
            if (tpTag) tpTag.transform.localPosition = new Vector3(0f, offset, 0f);
        }

        private void UpdateFPSTexts(string fpsText)
        {
            if (fpFPSText != null) fpFPSText.text = fpsText;
            if (tpFPSText != null) tpFPSText.text = fpsText;
        }

        private void FixedUpdate()
        {
            if (nameTagHandler?.rig == null)
                return;

            string fpsText = $"{nameTagHandler.rig.fps} FPS";
            UpdateFPSTexts(fpsText);

            fpTextRenderer ??= fpTag?.transform.parent.GetComponent<Renderer>();
            tpTextRenderer ??= tpTag?.transform.parent.GetComponent<Renderer>();
            fpFPSRenderer ??= fpFPSText?.GetComponent<Renderer>();
            tpFPSRenderer ??= tpFPSText?.GetComponent<Renderer>();

            bool shouldRender = isFPSEnabled && fpTextRenderer != null && tpTextRenderer != null;

            if (fpFPSRenderer != null)
                fpFPSRenderer.forceRenderingOff = !shouldRender || fpTextRenderer.forceRenderingOff;

            if (tpFPSRenderer != null)
                tpFPSRenderer.forceRenderingOff = !shouldRender || tpTextRenderer.forceRenderingOff;

            if (fpTextRenderer != null)
            {
                Color currentColor = fpTextRenderer.material.color;
                if (fpFPSText != null) fpFPSText.color = currentColor;
                if (tpFPSText != null) tpFPSText.color = currentColor;
            }
        }
    }
}