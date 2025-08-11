using TMPro;
using UnityEngine;
using WhoIsTalking;
using WhoIsThatMonke.Classes;
using static WhoIsThatMonke.PublicVariablesGatherHere;

namespace WhoIsThatMonke.Handlers
{
    public class ColorHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;

        private GameObject fpTag, tpTag;
        private GameObject firstPersonNameTag, thirdPersonNameTag;

        private Renderer fpColorRenderer, tpColorRenderer;
        private Renderer fpTextRenderer, tpTextRenderer;

        private TextMeshPro fpColorText, tpColorText;

        private Shader uiShader;
        private OffsetCalculatorCoolKidzOnly offsetCalculator = new OffsetCalculatorCoolKidzOnly();

        private void Awake() => uiShader = Shader.Find("UI/Default");

        private void Start()
        {
            CreateColorTags();
            BoolChangedButOnlyTheGoodOnes += CalculateDaOffset;
        }

        private void CalculateDaOffset()
        {
            offsetCalculator.ClearBoolsForDaSools();
            offsetCalculator.AddBool(isVelocityEnabled);
            offsetCalculator.AddBool(isFPSEnabled);

            float offset = offsetCalculator.CalculateOffsetCoolKidz();

            if (fpTag != null) fpTag.transform.localPosition = new Vector3(0f, offset, 0f);
            if (tpTag != null) tpTag.transform.localPosition = new Vector3(0f, offset, 0f);
        }

        private string GetColorCode(VRRig rig)
        {
            Color color = rig.playerColor;
            int multiplier = twoFiftyFiveColorCodes ? 255 : 9;

            int r = Mathf.RoundToInt(color.r * multiplier);
            int g = Mathf.RoundToInt(color.g * multiplier);
            int b = Mathf.RoundToInt(color.b * multiplier);

            return $"{r}, {g}, {b}";
        }

        private void CreateColorTags()
        {
            SetupTag(isFirstPerson: true, ref firstPersonNameTag, ref fpTag, ref fpColorRenderer, ref fpColorText, out fpTextRenderer);

            SetupTag(
                isFirstPerson: false, ref thirdPersonNameTag, ref tpTag, ref tpColorRenderer, ref tpColorText, out tpTextRenderer);

            UpdateColorPatchText();
        }

        private void SetupTag(bool isFirstPerson, ref GameObject nameTag, ref GameObject tagObject, ref Renderer colorRenderer, ref TextMeshPro colorText, out Renderer textRenderer)
        {
            string parentName = isFirstPerson ? "First Person NameTag" : "Third Person NameTag";
            Transform parent = transform.FindChildRecursive(parentName)?.FindChildRecursive("NameTag");

            if (parent == null)
            {
                textRenderer = null;
                return;
            }

            nameTag = parent.gameObject;

            tagObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            tagObject.name = (isFirstPerson ? "FP" : "TP") + " Color Holder";
            tagObject.transform.SetParent(nameTag.transform);
            tagObject.transform.localPosition = new Vector3(0f, 4f, 0f);
            tagObject.transform.localScale = Vector3.one;
            tagObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            tagObject.layer = nameTag.layer;

            Destroy(tagObject.GetComponent<Collider>());

            colorRenderer = tagObject.GetComponent<Renderer>();
            colorRenderer.material = new Material(uiShader);

            colorText = tagObject.AddComponent<TextMeshPro>();
            colorText.alignment = TextAlignmentOptions.Center;
            colorText.transform.rotation = Quaternion.Euler(0, 180, 0);
            colorText.font = nameTagHandler.rig.playerText1.font;
            colorText.fontSize = 7;
            colorText.text = GetColorCode(nameTagHandler.rig);
            colorText.color = nameTagHandler.rig.playerColor;

            textRenderer = parent.GetComponent<Renderer>();
        }

        private void UpdateColorPatchText()
        {
            string colorCode = GetColorCode(nameTagHandler.rig);

            if (fpColorText != null)
                fpColorText.text = colorCode;

            if (tpColorText != null)
                tpColorText.text = colorCode;
        }

        private void FixedUpdate()
        {
            if (nameTagHandler == null) return;

            string currentColorCode = GetColorCode(nameTagHandler.rig);

            if (fpColorText != null && fpColorText.text != currentColorCode)
                fpColorText.text = currentColorCode;

            if (tpColorText != null && tpColorText.text != currentColorCode)
                tpColorText.text = currentColorCode;

            if (isColorCodeEnabled)
            {
                if (fpColorRenderer != null && fpTextRenderer != null)
                    fpColorRenderer.forceRenderingOff = fpTextRenderer.forceRenderingOff;

                if (tpColorRenderer != null && tpTextRenderer != null)
                    tpColorRenderer.forceRenderingOff = tpTextRenderer.forceRenderingOff;
            }
            else
            {
                if (fpColorRenderer != null) fpColorRenderer.forceRenderingOff = true;
                if (tpColorRenderer != null) tpColorRenderer.forceRenderingOff = true;
            }

            if (fpTextRenderer != null)
            {
                Color actualColor = fpTextRenderer.material.color;
                if (fpColorText != null) fpColorText.color = actualColor;
                if (tpColorText != null) tpColorText.color = actualColor;
            }
        }
    }
}