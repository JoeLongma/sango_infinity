namespace Sango.Game.Render
{
    public class WindowEvent : RenderEventBase
    {
        public string windowName;
        public object arg1;
        public object arg2;
        public object arg3;
        public object arg4;
        public object arg5;
        public object arg6;
        public object arg7;
        public override void Enter(Scenario scenario)
        {
            Window.WindowInterface window = Window.Instance.Open(windowName);
            if (window == null) return;
        }

        public override void Exit(Scenario scenario)
        {
            Window.Instance.Close(windowName);
        }
    }
}
