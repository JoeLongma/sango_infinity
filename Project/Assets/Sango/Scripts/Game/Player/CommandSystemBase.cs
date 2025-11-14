namespace Sango.Game.Player
{
    public class CommandSystemBase<T> : System<T>, ICommandEvent where T : Module, new()
    {
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
        public virtual void OnBack() {; }

        /// <summary>
        /// 当前命令被舍弃的时候触发
        /// </summary>
        public virtual void OnDestroy() {; }

        /// <summary>
        /// 结束整个命令链的时候触发
        /// </summary>
        public virtual void OnDone() {; }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public virtual void Update() {; }

        public virtual void HandleEvent(CommandEventType eventType, Cell cell)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                    PlayerCommand.Instance.Back(); break;
            }

        }

        public virtual bool IsDone { get; protected set; }
    }
}
