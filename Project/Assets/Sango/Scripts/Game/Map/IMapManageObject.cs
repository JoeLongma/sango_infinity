using UnityEngine;
namespace Sango.Game
{
    public interface IMapManageObject
    {
        Sango.Render.MapRender manager { get; set; }
        Sango.Tools.Rect bounds { get; set; }
        Sango.Tools.Rect worldBounds { get;}
        UnityEngine.Transform transform { get; }
        int objId { get; set; }
        int objType { get; set; }
        int bindId { get; set; }
        string modelAsset { get; set; }

        int modelId { get; set; }
        bool visible { get; set; }
        bool isStatic { get; set; }
        bool selectable { get; set; }
        bool remainInView { get; set; }

        Vector3 position { get; set; }
        Vector3 rotation { get; set; }
        Vector3 forward { get; set; }
        Vector3 scale { get; set; }
        Vector2Int coords { get; set; }
        void OnClick();
        bool Overlaps(Sango.Tools.Rect rect);
        void OnPointerEnter();
        void OnPointerExit();
        void SetOutlineShow(Material material);
        void EditorShow(bool b);
        void SetParent(Transform parent);
        void SetParent(Transform parent, bool worldPositionStays);
        void Destroy();

        GameObject GetGameObject();

        public void CreateModel(string meshFile, string textureFile, string shaderName, bool isShareMat = true);
        public void CreateModel(string packagePath, string assetName);
        public void CreateModel(string assetName);
        public void CreateModel(UnityEngine.Object modelObj);
        public void ChangeModel(string newAsset);

        public void ReLoadModels(bool checkAsset = true);

        public void ClearModels();
        public void ReCheckVisible();
    }
}
