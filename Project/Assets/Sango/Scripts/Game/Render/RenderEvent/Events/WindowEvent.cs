using Sango.Game.Render.UI;

namespace Sango.Game.Render
{
    public class WindowEvent : RenderEventBase
    {
        public string windowName;
        public object[] args;

        Window.WindowInterface targetWindow;
        public override void Enter(Scenario scenario)
        {
            targetWindow = Window.Instance.Open(windowName, args);
            if (targetWindow == null) return;
            targetWindow.OnHideAction = OnWindowHide;
        }

        void OnWindowHide()
        {
            targetWindow.OnHideAction = null;
        }
    }
}
