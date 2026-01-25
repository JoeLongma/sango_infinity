using UnityEngine;

namespace RTEditor
{
    public class MeshRayHit
    {   //主要用于处理射线（Ray）与三维网格（Mesh）的碰撞检测，并提供射线和网格碰撞的相关信息
        #region Private Variables
        private Ray _ray;   		//代表射线的实例
        private float _hitEnter;    //代表射线与网格的碰撞距离或者是一个时间戳
        private int _hitTraingleIndex;  //代表碰撞的三角形索引
        private Vector3 _hitPoint;  //代表碰撞点在三维空间中的位置
        private Vector3 _hitNormal; //代表碰撞点在表面的法向量（向量垂直于表面）
        #endregion

        #region Public Properties
        public Ray Ray { get { return _ray; } }     			//返回射线的实例   
        public float HitEnter { get { return _hitEnter; } }     //返回射线与网格的碰撞距离或者时间戳
        public int HitTriangleIndex { get { return _hitTraingleIndex; } }       //返回碰撞的三角形索引
        public Vector3 HitPoint { get { return _hitPoint; } }   //返回碰撞点在三维空间中的位置
        public Vector3 HitNormal { get { return _hitNormal; } } //返回碰撞点在表面的法向量
        #endregion

        #region Constructors
        public MeshRayHit(Ray ray, float hitEnter, int hitTriangleIndex, Vector3 hitPoint, Vector3 hitNormal)
        {   //构造函数，创建 MeshRayHit 类的实例象
            _ray = ray;
            _hitEnter = hitEnter;
            _hitTraingleIndex = hitTriangleIndex;
            _hitPoint = hitPoint;

            _hitNormal = hitNormal;
            _hitNormal.Normalize();
        }
        #endregion
    }
}