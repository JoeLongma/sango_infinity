namespace Sango.Game
{
    public abstract class TroopActionBase : ActionBase
    {
        protected Force Force { get; set; }
        protected Troop Troop { get; set; }
        protected int[] Params { get; set; }
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            Troop = sangoObjects[0] as Troop;
            if(Troop == null)
                Force = sangoObjects[0] as Force;
            Params = p;
        }
    }
}
