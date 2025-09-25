using UnityEngine;

namespace Sango.Game.Render
{
    public class FlagRender : MonoBehaviour
    {
        public float flagW = 25;
        public float flagH = 19;
        public int xCount = 10;
        public float flagTexWidth = 256;
        public float flagTexHeight = 256;
        public int flagId = 40;

        public void Init(Force force)
        {
            Renderer renderer = GetComponentInChildren<Renderer>(true);
            Material outlineMat = renderer.materials[0];
            Material baseMat = renderer.materials[1];
            Material TextMat = renderer.materials[2];
            TextMat.SetTexture("_MainTex", TextFactory.Instance.GetTexture(force.Name.Substring(0, 1), 40));

            Vector2 textScale = new Vector2(flagW / flagTexWidth, flagH / flagTexHeight);
            outlineMat.SetTextureScale("_MainTex", textScale);
            baseMat.SetTextureScale("_MainTex", textScale);

            int final_flag_id = flagId + force.Flag.Id % 6;

            int x = final_flag_id % xCount;
            int y = final_flag_id / xCount;
            Vector2 textOffset = new Vector2(x * (flagW / flagTexWidth) - 0.003f, -y * (flagH / flagTexHeight));
            outlineMat.SetTextureOffset("_MainTex", textOffset);
            baseMat.SetTextureOffset("_MainTex", textOffset);
            baseMat.SetColor("_Color", force.Flag.color);
        }

        public void Init(Troop troop)
        {
            Renderer renderer = GetComponentInChildren<Renderer>(true);
            Material outlineMat = renderer.materials[0];
            Material baseMat = renderer.materials[1];
            Material TextMat = renderer.materials[2];
            TextMat.SetTexture("_MainTex", TextFactory.Instance.GetTexture(troop.Name.Substring(0, 1), 40));

            Vector2 textScale = new Vector2(flagW / flagTexWidth, flagH / flagTexHeight);
            outlineMat.SetTextureScale("_MainTex", textScale);
            baseMat.SetTextureScale("_MainTex", textScale);
            int final_flag_id = flagId + troop.BelongForce.Flag.Id % 6;

            int x = final_flag_id % xCount;
            int y = final_flag_id / xCount;

            baseMat.SetColor("_Color", troop.BelongForce.Flag.color);

            Vector2 textOffset = new Vector2(x * (flagW / flagTexWidth) - 0.003f, -y * (flagH / flagTexHeight));
            outlineMat.SetTextureOffset("_MainTex", textOffset);
            baseMat.SetTextureOffset("_MainTex", textOffset);

        }

        [ContextMenu("test")]
        public void Test()
        {
            Renderer renderer = GetComponentInChildren<Renderer>(true);
            Material outlineMat = renderer.sharedMaterials[0];
            Material baseMat = renderer.sharedMaterials[1];
            Material TextMat = renderer.sharedMaterials[2];

            Vector2 textScale = new Vector2(flagW / flagTexWidth, flagH / flagTexHeight);
            outlineMat.SetTextureScale("_MainTex", textScale);
            baseMat.SetTextureScale("_MainTex", textScale);
            int x = flagId % xCount;
            int y = flagId / xCount;

            Vector2 textOffset = new Vector2(x * (flagW / flagTexWidth) - 0.003f, -y * (flagH / flagTexHeight) -0.075f);
            outlineMat.SetTextureOffset("_MainTex", textOffset);
            baseMat.SetTextureOffset("_MainTex", textOffset);

        }
    }
}
