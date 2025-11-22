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
            if (arg7 != null)
            {
                window.Call("InitEvent", this, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                return;
            }
            if (arg6 != null)
            {
                window.Call("InitEvent", this, arg1, arg2, arg3, arg4, arg5, arg6);
                return;
            }
            if (arg5 != null)
            {
                window.Call("InitEvent", this, arg1, arg2, arg3, arg4, arg5);
                return;
            }
            if (arg4 != null)
            {
                window.Call("InitEvent", this, arg1, arg2, arg3, arg4);
                return;
            }
            if (arg3 != null)
            {
                window.Call("InitEvent", this, arg1, arg2, arg3);
                return;
            }
            if (arg2 != null)
            {
                window.Call("InitEvent", this, arg1, arg2);
                return;
            }
            if (arg1 != null)
            {
                window.Call("InitEvent", this, arg1);
                return;
            }
        }

        public override void Exit(Scenario scenario)
        {
            Window.Instance.Close(windowName);
        }
    }
}
