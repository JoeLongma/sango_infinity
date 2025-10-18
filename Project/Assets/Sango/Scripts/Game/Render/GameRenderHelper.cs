using Sango.Loader;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game

{
    public class GameRenderHelper
    {
        public static string HeadIconPath = "Assets/UI/AtlasTexture/Face";
        public static string TroopHeadbarRes = "Assets/UI/Prefab/window_troop_bar.prefab";
        public static string CityHeadbarRes = "Assets/UI/Prefab/window_city_bar.prefab";
        public static string BuildingHeadbarRes = "Assets/UI/Prefab/window_building_bar.prefab";
        public static string[] CityResPath = new string[]{
        "Assets/Model/Prefab/p_city_1.prefab",
        "Assets/Model/Prefab/p_city_2.prefab",
        };

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

        public static string GetCityModelAsset(int type)
        {
            if (type < 0 || type >= CityResPath.Length)
                type = 0;
            return CityResPath[type];
        }
    }
}
