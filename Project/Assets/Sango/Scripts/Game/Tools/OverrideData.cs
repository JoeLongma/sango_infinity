namespace Sango.Game.Tools
{
    public class OverrideData<T>
    {
        public T Value { get; set; }
        public OverrideData(T baseValue) { Value = baseValue; }
    }
}
