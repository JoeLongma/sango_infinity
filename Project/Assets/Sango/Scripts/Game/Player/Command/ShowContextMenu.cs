using Sango.Render;
using UnityEngine;

namespace Sango.Game.Player
{
    public class ShowContextMenu : CommandBase
    {
        public Vector2 screenPos;

        public override void OnBack()
        {
            Window.Instance.ShowWindow("window_contextMenu");
        }

        public override void OnDestroy()
        {
            Window.Instance.HideWindow("window_contextMenu");
        }

        public override void OnDone()
        {
            Window.Instance.HideWindow("window_contextMenu");
        }

        public override void OnEnter()
        {
            Sango.Game.Render.UI.ContextMenu.Show(screenPos);
        }

        public override void OnExit()
        {
            Window.Instance.HideWindow("window_contextMenu");
        }
        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameController.Instance.IsOverUI()) return;
                Sango.Game.Render.UI.ContextMenu.Close();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (GameController.Instance.IsOverUI()) return;
                MapObject mapObject = GameController.Instance.CheckMouseIsOnMapObject(out Vector3 hitPoint);
                if (mapObject == null)
                {
                    PlayerCommand.Instance.Done();
                    return;
                }

                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, hitPoint);
                Sango.Game.Render.UI.ContextMenu.Show(screenPos);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (Sango.Game.Render.UI.ContextMenu.Close())
                    PlayerCommand.Instance.Done();
            }
        }
    }
}
