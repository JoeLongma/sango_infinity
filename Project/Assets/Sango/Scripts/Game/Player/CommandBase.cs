namespace Sango.Game.Player
{
    public class CommandBase
    {
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

        public virtual void Update()
        {
            

        }



        public virtual bool IsDone { get; protected set; }
    }
}
