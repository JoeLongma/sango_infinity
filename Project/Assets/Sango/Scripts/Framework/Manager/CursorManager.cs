using Sango.Loader;
using UnityEngine;

namespace Sango
{
    public class CursorManager : Singleton<CursorManager> 
    {

        // 存储所有鼠标纹理
        private static Texture2D[] cursorTextures = new Texture2D[24];

        // 鼠标热点配置
        private static Vector2[] cursorHotSpots = new Vector2[24];

        // 鼠标样式名称
        private static string[] cursorNames = new string[24]
        {
            "Default",            // 4847-1 默认箭头光标
            "ArrowUpLeft",        // 4847-2 左上箭头
            "ArrowUp",            // 4847-3 上箭头
            "ArrowUpRight",       // 4847-4 右上箭头
            "ArrowRight",         // 4847-5 右箭头
            "ArrowDownRight",     // 4847-6 右下箭头
            "ArrowDown",          // 4847-7 下箭头
            "ArrowDownLeft",      // 4847-8 左下箭头
            "ArrowLeft",          // 4847-9 左箭头
            "Pointer",            // 4847-10 手型指针（链接/可点击）
            "Grab",               // 4847-11 抓取（未激活）
            "Grabbing",           // 4847-12 抓取（激活）
            "NoDrop",             // 4847-13 禁止放置/操作
            "CrosshairBlue",      // 4847-14 蓝色十字准星（鼠标左键按下激活，拖拽窗口标题栏）
            "Crosshair",          // 4847-15 白色十字准星（默认，四向移动）
            "CrosshairTarget",    // 4847-16 目标瞄准十字
            "CrosshairResize",    // 4847-18 十字调整大小（斜向）
            "ResizeHorizontal",   // 4847-18 水平调整大小
            "ResizeDiagonalNWSE", // 4847-19 对角调整大小（左上-右下）（西北-东南）
            "ResizeDiagonalNESW", // 4847-20 对角调整大小（右上-左下）（东北-西南）
            "ResizeVertical",     // 4847-21 垂直调整大小
            "BuildingIdentify",   // 4847-22 建筑识别
            "UnitIdentify",       // 4847-23 部队识别
            "CommissionIdentify"  // 4847-24 委任识别
        };

        // 资源路径配置
        public static string cursorResourcePath = "Assets/Cursor/";

        // 初始化加载所有鼠标纹理
        public void InitCursorTextures()
        {
            // 加载所有鼠标纹理
            for (int i = 0; i < cursorNames.Length; i++)
            {
                cursorTextures[i] = ObjectLoader.LoadObject<UnityEngine.Texture2D>(cursorResourcePath + $"4847-{i + 1}.png");

                if (cursorTextures[i] != null)
                {
                    // 设置热点位置，可根据实际情况调整
                    cursorHotSpots[i] = GetDefaultHotSpot(i);
                }
            }

            // 设置默认鼠标样式
            SetCursorStyle(0);
        }

        // 获取默认热点位置
        private Vector2 GetDefaultHotSpot(int index)
        {
            // 基于鼠标类型的通用热点配置
            switch (index)
            {
                // 箭头类光标（热点通常在尖端）
                case 0:
                    return new Vector2(5, 0);
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    return new Vector2(2, 2);

                // 手型和抓取类
                case 9:
                case 10:
                case 11:
                    return new Vector2(10, 2);

                // 禁止类
                case 12:
                    return new Vector2(15, 15);

                // 十字准星类
                case 13:
                case 14:
                case 15:
                case 16:
                    return new Vector2(cursorTextures[index]?.width / 2 ?? 15,
                                       cursorTextures[index]?.height / 2 ?? 15);

                // 调整大小类
                case 17:
                case 18:
                case 19:
                case 20:
                    return new Vector2(15, 15);

                // 识别类光标
                case 21:
                case 22:
                case 23:
                    return new Vector2(cursorTextures[index]?.width / 2 ?? 15,
                                       cursorTextures[index]?.height / 2 ?? 15);

                default:
                    return Vector2.zero;
            }
        }

        // 通过索引切换鼠标样式
        public void SetCursorStyle(int index)
        {
            if (index >= 0 && index < cursorTextures.Length && cursorTextures[index] != null)
            {
                Cursor.SetCursor(cursorTextures[index], cursorHotSpots[index], CursorMode.Auto);
            }
            else
            {
                Debug.LogWarning($"无效的光标索引: {index}");
                ResetToSystemDefault();
            }
        }

        // 通过名称切换鼠标样式
        public void SetCursorStyle(string style)
        {
            for (int i = 0; i < cursorNames.Length; i++)
            {
                if (cursorNames[i] == style)
                {
                    SetCursorStyle(i);
                    return;
                }
            }
            Debug.LogWarning($"未找到光标索引: {style}");
            ResetToSystemDefault();
        }

        // 重置为系统默认光标
        private void ResetToSystemDefault()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // 恢复系统默认
        }

        // 常用鼠标样式快捷方法
        public void SetDefaultCursor() => SetCursorStyle(0); 	 // Normal
        public void SetPointCursor() => SetCursorStyle(9);       // Point
        public void SetGrabCursor() => SetCursorStyle(10);       // Grab
        public void SetGrabbingCursor() => SetCursorStyle(11);   // Grabbing
        public void SetNoDropCursor() => SetCursorStyle(12);
        public void SetCrosshairCursor() => SetCursorStyle(14);  // Crosshair
        public void SetResizeHorizontalCursor() => SetCursorStyle(18);
        public void SetResizeVerticalCursor() => SetCursorStyle(20);
        public void HideCursor() => Cursor.visible = false;
        public void ShowCursor() => Cursor.visible = true;

        // 获取鼠标样式数量
        public int GetCursorCount() => cursorTextures.Length;

        // 获取鼠标样式名称
        public string GetCursorName(int index)
        {
            if (index >= 0 && index < cursorNames.Length)
                return cursorNames[index];
            return "Unknown";
        }

        // 获取鼠标样式索引
        public int GetCursorIndex(string name)
        {
            for (int i = 0; i < cursorNames.Length; i++)
            {
                if (cursorNames[i] == name)
                    return i;
            }
            return -1;
        }
    }
}