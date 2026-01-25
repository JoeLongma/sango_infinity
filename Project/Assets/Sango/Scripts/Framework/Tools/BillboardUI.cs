using Sango.Render;
using UnityEngine;

namespace Sango
{
    public class BillboardUI : MonoBehaviour
    {
        public Vector3 initScale = new Vector3(-1f, 1f, 1f);
        public Vector2 scaleFactor = Vector2.one;
        public Vector3 offsetFactor = new Vector3(0f, 0f, 0f);
        public Vector3 cacheOffset = new Vector3(0f, 0f, 0f);
        private Transform cacheTrans;
        float tempFactor;

        private void Start()
        {
            CatchMainCamera();
            tempFactor = -1;
        }

        private void OnDisable()
        {
            tempFactor = -1;
        }

        private void OnEnable()
        {
            tempFactor = -1;
        }

        void CatchMainCamera()
        {
            if (cacheTrans == null)
                cacheTrans = Camera.main.transform;
        }

        public void Update()
        {
            CatchMainCamera();
            transform.LookAt(transform.position + cacheTrans.rotation * Vector3.back, cacheTrans.rotation * Vector3.up);
            float factor = MapRender.Instance.mapCamera.cameraDistanceFactor;
            if (factor != tempFactor)
            {
                tempFactor = factor;
                // 使用计算得出的缩放因子，通过线性插值（Lerp）方法来调整该UI元素对象的缩放比例
                // 在初始缩放向量（initScale）乘以x方向缩放因子（scaleFactor.x）和初始缩放向量乘以y方向缩放因子（scaleFactor.y）之间进行插值，使缩放比例根据factor的值动态变化
                transform.localScale = Vector3.Lerp(initScale * scaleFactor.x, initScale * scaleFactor.y, factor);
                transform.localPosition = cacheOffset + Vector3.Lerp(offsetFactor, Vector3.zero, factor);
            }
        }
    }
}
