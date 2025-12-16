using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Player
{

    public class ScaleWithScreenHeigh : MonoBehaviour
    {
        public float originalHeight = 1080;
        private void Start()
        {
            float s = Game.Instance.UIRoot.GetComponent<CanvasScaler>().referenceResolution.y / originalHeight;
            if (s < 1)
                transform.localScale = new Vector3(s, s, s);
        }
    }
}
