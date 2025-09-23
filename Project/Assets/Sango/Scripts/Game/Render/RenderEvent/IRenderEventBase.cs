namespace Sango.Game.Render
{
    public interface IRenderEventBase
    {
        bool IsDone { get; set; }
        bool IsVisible();
        bool Update(Scenario scenario, float deltaTime);
        void Enter(Scenario scenario);
        void Exit(Scenario scenario);
    }
}
