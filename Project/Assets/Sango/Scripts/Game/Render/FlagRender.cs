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

            int x = force.Flag.Id % xCount;
            int y = force.Flag.Id / xCount;
            Vector2 textOffset = new Vector2(x * (flagW / flagTexWidth) - 0.003f, -y * (flagH / flagTexHeight));
            outlineMat.SetTextureOffset("_MainTex", textOffset);
            baseMat.SetTextureOffset("_MainTex", textOffset);

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

            int x = troop.BelongForce.Flag.Id % xCount;
            int y = troop.BelongForce.Flag.Id / xCount;
            Vector2 textOffset = new Vector2(x * (flagW / flagTexWidth) - 0.003f, -y * (flagH / flagTexHeight));
            outlineMat.SetTextureOffset("_MainTex", textOffset);
            baseMat.SetTextureOffset("_MainTex", textOffset);
        }
    }
}
