namespace Sango.Game.Player
{
    public class CommandSystemBase : Module, ICommandEvent
    {
        public virtual void Push()
        {
            GameSystemManager.Instance.Push(this);
        }

        public virtual void Back()
        {
            if (GameSystemManager.Instance.CurrentCommand == this)
                GameSystemManager.Instance.Back();
        }

        public virtual void Init() {; }
        public virtual void Clear() {; }

        /// <summary>
        /// 进入当前命令的时候触发
        /// </summary>
        public virtual void OnEnter() {; }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public virtual void OnExit() {; }

        /// <summary>
        /// 当前命令被重新拾起的时候触发(返回)
        /// </summary>
        public virtual void OnBack(ICommandEvent whoGone) {; }

        /// <summary>
        /// 当前命令被舍弃的时候触发
        /// </summary>
        public virtual void OnDestroy() {; }

        /// <summary>
        /// 结束整个命令链的时候触发
        /// </summary>
        public virtual void OnDone() { OnDestroy(); }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public virtual void Update() {; }

        public virtual void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                    GameSystemManager.Instance.Back(); break;
            }

        }

        public virtual void Exit()
        {
            if (GameSystemManager.Instance.CurrentCommand == this)
            {
                GameSystemManager.Instance.Back();
            }
        }

        public virtual void Done()
        {
            if (GameSystemManager.Instance.CurrentCommand == this)
            {
                GameSystemManager.Instance.Done();
            }
        }

        public virtual bool IsDone { get; protected set; }

        public virtual bool IsValid { get; protected set; }
    }
}
