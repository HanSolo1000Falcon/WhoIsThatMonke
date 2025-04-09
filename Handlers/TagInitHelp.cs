using UnityEngine;

namespace WhoIsThatMonke.Handlers {
    public class TagInitHelp {
        public static (Renderer, TextMeshPro) InitTag(GameObject tagCreated, GameObject parent) {
            tagCreated.transform.SetParent(parent.transform);
            tagCreated.transform.localPosition = new Vector3(0f, 4f, 0f);
            tagCreated.transform.localScale = Vector3.one;
            tagCreated.transform.localRotation = Quaternion.Euler(0, 180, 0);
            tagCreated.layer = parent.layer;

            Destroy(tagCreated.GetComponent<Collider>());

            Renderer returnRend = tagCreated.GetComponent<Renderer>();
            returnRand.material = new Material(uiShader);

            TextMeshPro tagTXTCreated = tagCreated.AddComponent<TextMeshPro>();
            tagTXTCreated.alignment = TextAlignmentOptions.Center;
            tagTXTCreated.transform.rotation = Quaternion.Euler(0, 180, 0);
            tagTXTCreated.font = nameTagHandler.rig.playerText1.font;
            tagTXTCreated.fontSize = 7;
            tagTXTCreated.text = GetColorCode(nameTagHandler.rig);
            tagTXTCreated.color = nameTagHandler.rig.mainSkin.material.color;
        }
    }
}