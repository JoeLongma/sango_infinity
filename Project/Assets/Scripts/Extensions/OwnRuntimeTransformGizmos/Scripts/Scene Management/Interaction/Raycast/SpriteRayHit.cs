using UnityEngine;

namespace RTEditor
{
    public class SpriteRayHit
    {   //用于处理射线与精灵的碰撞检测，并提供射线和精灵碰撞的相关信息
        #region Private Variables
        private Ray _ray;           	//代表射线的实例
        private float _hitEnter;    	//代表射线与精灵或其他对象的碰撞距离
        private SpriteRenderer _hitSpriteRenderer;  //代表精灵渲染器的实例
        private Vector3 _hitPoint;      //代表碰撞点在三维空间中的位置
        private Vector3 _hitNormal;     //代表碰撞点在表面的法向量（向量垂直于表面）
        #endregion

        #region Public Properties   
        public Ray Ray { get { return _ray; } }     			//返回射线的实例
        public float HitEnter { get { return _hitEnter; } }     //返回射线与精灵或其他对象的碰撞距离
        public SpriteRenderer HitSpriteRenderer {get{return _hitSpriteRenderer;} }       //返回精灵渲染器的实例
        public Vector3 HitPoint { get { return _hitPoint; } }       //返回碰撞点在三维空间中的位置
        public Vector3 HitNormal { get { return _hitNormal; } }     //返回碰撞点在表面的法向量
        #endregion

        #region Constructors
        public SpriteRayHit(Ray ray, float hitEnter, SpriteRenderer hitSpriteRenderer, Vector3 hitPoint, Vector3 hitNormal)
        {   //构造函数，用于创建 SpriteRayHit 类的实例
            _ray = ray;
            _hitEnter = hitEnter;
            _hitSpriteRenderer = hitSpriteRenderer;
            _hitPoint = hitPoint;

            _hitNormal = hitNormal;
            _hitNormal.Normalize(); //对碰撞法向量进行了归一化处理，使其为单位向量
        }
        #endregion
    }
}