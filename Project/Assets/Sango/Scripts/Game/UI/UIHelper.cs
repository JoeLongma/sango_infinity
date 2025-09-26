using Sango.Loader;
using UnityEngine.UI;

namespace Sango.Game.UI
{
    public class UIHelper
    {
        public static string HeadIconPath = "Assets/UI/AtlasTexture/Face";
        public static UnityEngine.Sprite LoadHeadIcon(int id)
        {
            return LoadHeadIcon(id, 2);
        }
        public static UnityEngine.Sprite LoadHeadIcon(int id, int type)
        {
            string headPath = $"{HeadIconPath}/{id}_{type}.png";
            UnityEngine.Sprite headSpr = ObjectLoader.LoadObject<UnityEngine.Sprite>(headPath);
            if (headSpr == null)
            {
                headPath = $"{HeadIconPath}/0_{type}.png";
                headSpr = ObjectLoader.LoadObject<UnityEngine.Sprite>(headPath);
            }
            return headSpr;
        }
    }
}
