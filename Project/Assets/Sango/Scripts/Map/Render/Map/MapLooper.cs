using UnityEngine;

namespace Sango.Render
{
    public class MapLooper : MonoBehaviour
    {
        //public float depthDistance = 3500;
        // float lastDepthDistance = 3500;
        private void Update()
        {
#if UNITY_EDITOR
            if (MapRender.Instance != null)
                MapRender.Instance.Update();
#else
            MapRender.Instance.Update();
#endif

            //if (lastDepthDistance != depthDistance)
            //{
            //    lastDepthDistance = depthDistance;
            //    Shader.SetGlobalFloat("_DepthDistance", depthDistance);
            //}
        }
    }
}
