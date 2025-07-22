using TMPro;
using UnityEngine;
using WhoIsTalking;
using System.Globalization;
using static WhoIsThatMonke.PublicVariablesGatherHere;

namespace WhoIsThatMonke.Handlers
{
    internal class VelocityHandler : MonoBehaviour
    {
        public NameTagHandler nameTagHandler;

        private float lastTime;
        private float cooldown = 0.5f;

        private GameObject fpTag, tpTag;
        private GameObject firstPersonNameTag, thirdPersonNameTag;

        private TextMeshPro fpVelocityText, tpVelocityText;
        private Renderer fpVelocityRenderer, tpVelocityRenderer;
        private Renderer fpTextRenderer, tpTextRenderer;

        private Shader uiShader;

        private void Awake() => uiShader = Shader.Find("UI/Default");
        private void Start() => CreateVelocityTagsIfNeeded();

        private void CreateVelocityTagsIfNeeded()
        {
            if (firstPersonNameTag == null)
            {
                firstPersonNameTag = transform.FindChildRecursive("First Person NameTag")?.FindChildRecursive("NameTag")?.gameObject;
                if (firstPersonNameTag != null)
                    CreateVelocityTag(ref fpTag, ref fpVelocityText, ref fpVelocityRenderer, firstPersonNameTag);
            }

            if (thirdPersonNameTag == null)
            {
                thirdPersonNameTag = transform.FindChildRecursive("Third Person NameTag")?.FindChildRecursive("NameTag")?.gameObject;
                if (thirdPersonNameTag != null)
                    CreateVelocityTag(ref tpTag, ref tpVelocityText, ref tpVelocityRenderer, thirdPersonNameTag);
            }

            UpdateVelocityTexts("0.0", Color.green);
        }

        private void CreateVelocityTag(ref GameObject tagObj, ref TextMeshPro textObj, ref Renderer rendererObj, GameObject parent)
        {
            tagObj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            tagObj.name = $"{(parent == firstPersonNameTag ? "FP" : "TP")} Velocity Holder";
            tagObj.transform.SetParent(parent.transform);
            tagObj.transform.localPosition = new Vector3(0f, 2f, 0f);
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
        }

        private void UpdateVelocityTexts(string velocityStr, Color color)
        {
            if (fpVelocityText != null)
            {
                fpVelocityText.text = velocityStr;
                fpVelocityText.color = color;
            }

            if (tpVelocityText != null)
            {
                tpVelocityText.text = velocityStr;
                tpVelocityText.color = color;
            }
        }

        private void FixedUpdate()
        {
            if (nameTagHandler?.rig == null)
                return;

            lastTime += Time.deltaTime;

            if (lastTime >= cooldown)
            {
                lastTime = 0f;
                Vector3 velocityVector = nameTagHandler.rig.LatestVelocity();
                float speed = velocityVector.magnitude;
                string velocityStr = speed.ToString("F1", CultureInfo.InvariantCulture);

                Color color = speed switch
                {
                    < 6.5f => Color.green,
                    < 7.5f => Color.yellow,
                    _ => Color.red
                };

                UpdateVelocityTexts(velocityStr, color);
            }

            fpTextRenderer ??= fpTag?.transform.parent.GetComponent<Renderer>();
            tpTextRenderer ??= tpTag?.transform.parent.GetComponent<Renderer>();
            fpVelocityRenderer ??= fpVelocityText?.GetComponent<Renderer>();
            tpVelocityRenderer ??= tpVelocityText?.GetComponent<Renderer>();

            bool shouldRender = isVelocityEnabled && fpTextRenderer != null && tpTextRenderer != null;

            if (fpVelocityRenderer != null)
                fpVelocityRenderer.forceRenderingOff = !shouldRender || fpTextRenderer.forceRenderingOff;

            if (tpVelocityRenderer != null)
                tpVelocityRenderer.forceRenderingOff = !shouldRender || tpTextRenderer.forceRenderingOff;
        }
    }
}