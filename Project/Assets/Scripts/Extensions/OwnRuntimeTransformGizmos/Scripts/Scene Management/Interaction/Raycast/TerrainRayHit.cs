using UnityEngine;

namespace RTEditor
{
    public class TerrainRayHit
    {   //用于处理地形和其他对象的射线碰撞检测，并提供射线和地形或其他对象碰撞的相关信息
        #region Private Variables   //私有变量
        private Ray _ray;           //代表射线的实例
        private float _hitEnter;    //代表射线与地形或其他对象的碰撞距离
        private Vector3 _hitPoint;  //代表碰撞点在三维空间中的位置
        private Vector3 _hitNormal; //代表碰撞点在表面的法向量（向量垂直于表面）
        #endregion

        #region Public Properties   //公共属性
        public Ray Ray { get { return _ray; } }                 //返回射线的实例
        public float HitEnter { get { return _hitEnter; } }     //返回射线与地形或其他对象的碰撞距离
        public Vector3 HitPoint { get { return _hitPoint; } }   //返回碰撞点在三维空间中的位置
        public Vector3 HitNormal { get { return _hitNormal; } } //返回碰撞点在表面的法向量
        #endregion

        #region Constructors    //构造函数
        public TerrainRayHit(Ray ray, RaycastHit raycastHit)
        {   //用于创建 TerrainRayHit 类的实例。
            _ray = ray;     //ray对象代表进行碰撞检测的射线
            _hitEnter = raycastHit.distance;

            _hitPoint = raycastHit.point;   //raycastHit对象包含射线和地形或其他对象碰撞的信息，如碰撞距离、碰撞点的位置和法向量等
            _hitNormal = raycastHit.normal;
        }
        #endregion
    }
}