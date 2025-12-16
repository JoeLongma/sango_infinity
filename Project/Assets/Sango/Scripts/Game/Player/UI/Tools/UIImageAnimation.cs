using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Player
{

    public class UIImageAnimation : MonoBehaviour
    {
        public UnityEngine.Sprite[] sprites;
        public Image image;
        public float speed;
        private int index = 0;
        private void Awake()
        {
            if (image == null)
                image = GetComponent<Image>();
            InvokeRepeating("UpdateRender", speed, speed);
        }

        private void UpdateRender()
        {
            index++;
            if (index >= sprites.Length)
                index = 0;
            if (image != null)
                image.sprite = sprites[index];
        }
    }
}
