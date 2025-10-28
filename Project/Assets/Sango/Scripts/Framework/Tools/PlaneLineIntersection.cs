using UnityEngine;

public static class PlaneLineIntersection
{
    /// <summary>
    /// 计算三点组成的平面与垂直于X轴的直线的交点
    /// </summary>
    /// <param name="p1">平面上的第一个点</param>
    /// <param name="p2">平面上的第二个点</param>
    /// <param name="p3">平面上的第三个点</param>
    /// <param name="linePoint">垂直于X轴的直线上的任意一点</param>
    /// <param name="lineDir">垂直于X轴的直线的方向向量（必须满足x分量为0，即与X轴垂直）</param>
    /// <param name="intersection">输出参数：交点坐标（若存在）</param>
    /// <returns>是否存在有效交点（true：存在；false：不存在，如平面无效、直线不垂直X轴、线面平行等）</returns>
    public static bool GetIntersectionWithXPerpendicularLine(
        Vector3 p1, Vector3 p2, Vector3 p3,
        Vector3 linePoint, Vector3 lineDir,
        out Vector3 intersection)
    {
        // 初始化输出参数（默认无效值）
        intersection = Vector3.zero;

        // --------------- 第一步：验证输入有效性 ---------------
        // 1. 检查三点是否共线（共线则无法确定平面）
        Vector3 v1 = p2 - p1; // 向量p1->p2
        Vector3 v2 = p3 - p1; // 向量p1->p3
        Vector3 normal = Vector3.Cross(v1, v2); // 平面法向量（由两向量叉积计算）
        if (normal.sqrMagnitude < Mathf.Epsilon) // 法向量模长接近0，说明三点共线
        {
            Debug.LogError("三点共线，无法确定平面！");
            return false;
        }

        // 2. 检查直线是否垂直于X轴（方向向量x分量必须为0，允许微小浮点数误差）
        if (Mathf.Abs(lineDir.x) > Mathf.Epsilon)
        {
            Debug.LogError("直线方向向量不垂直于X轴（x分量不为0）！");
            return false;
        }

        // 3. 检查直线方向向量是否为零向量（无效方向）
        if (lineDir.sqrMagnitude < Mathf.Epsilon)
        {
            Debug.LogError("直线方向向量为零向量，无效！");
            return false;
        }


        // --------------- 第二步：计算平面方程 ---------------
        // 平面一般方程：ax + by + cz + d = 0
        // 其中(a,b,c)为法向量normal，d通过平面上一点（如p1）计算
        float a = normal.x;
        float b = normal.y;
        float c = normal.z;
        float d = -(a * p1.x + b * p1.y + c * p1.z); // 代入p1到平面方程求解d


        // --------------- 第三步：计算直线参数方程 ---------------
        // 垂直于X轴的直线参数方程：
        // 任意点 = linePoint + t * lineDir （t为参数）
        // 因lineDir.x=0，直线上所有点的x坐标恒等于linePoint.x（核心特性）


        // --------------- 第四步：求解交点 ---------------
        // 交点需同时满足平面方程和直线方程，代入得：
        // a*(linePoint.x) + b*(linePoint.y + t*lineDir.y) + c*(linePoint.z + t*lineDir.z) + d = 0
        // 整理得：t = -[a*linePoint.x + b*linePoint.y + c*linePoint.z + d] / [b*lineDir.y + c*lineDir.z]

        // 计算分母（直线方向与平面法向量在Y-Z平面的投影点积）
        float denominator = b * lineDir.y + c * lineDir.z;
        if (Mathf.Abs(denominator) < Mathf.Epsilon) // 分母为0，说明直线与平面平行（无交点或共面）
        {
            Debug.LogWarning("直线与平面平行，无交点！");
            return false;
        }

        // 计算分子
        float numerator = a * linePoint.x + b * linePoint.y + c * linePoint.z + d;

        // 求解参数t
        float t = -numerator / denominator;

        // 计算交点坐标（代入直线参数方程）
        intersection = linePoint + t * lineDir;

        return true;
    }
}