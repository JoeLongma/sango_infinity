using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Player
{

    public class ScaleWithScreenHeigh : MonoBehaviour
    {
        private void Start()
        {
            float s = Game.Instance.CanvasScalerFactor;
            if (s < 1)
                transform.localScale = new Vector3(s, s, s);
        }
    }
}
