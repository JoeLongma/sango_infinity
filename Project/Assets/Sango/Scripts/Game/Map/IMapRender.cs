using UnityEngine;
namespace Sango.Game
{
    public interface IMapRender
    {
        public void AddDynamic(IMapManageObject obj);
        public void AddStatic(IMapManageObject obj);
        public void RemoveDynamic(IMapManageObject obj);
        public void RemoveStatic(IMapManageObject obj);
        public void AddInstance(IMapManageObject obj);
        public void RemoveInstance(IMapManageObject obj);
        public bool IsInView(IMapManageObject obj);
    }
}
