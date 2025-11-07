namespace Sango.Game
{
    public abstract class CityActionBase : ActionBase
    {
        protected Force Force { get; set; }
        protected int[] Params { get; set; }

        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            Force = sangoObjects[0] as Force;
            Params = p;
        }
    }
}
