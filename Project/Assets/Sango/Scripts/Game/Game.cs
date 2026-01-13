using Sango.Game.Action;
using Sango.Mod;
using Sango.Render;
using Sango.Tools;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game
{
    public class Game : App<Game>
    {
        public Camera UICamera { get; internal set; }
        public RectTransform UIRoot { get; internal set; }
        public Canvas RootCanvas { get; internal set; }
        public CanvasScaler CanvasScaler { get; internal set; }
        public float CanvasScalerFactor { get; internal set; }

        public override void Init(MonoBehaviour start, Platform.PlatformName targetPlatform)
        {
            CanvasScalerFactor = CanvasScaler.referenceResolution.y / 1080f;
            base.Init(start, targetPlatform);
            Window.Instance.Init(1024, 720);
            ActionBase.Init();
            Condition.Init();
            BuffEffect.Init();
            SkillEffect.Init();
            PersonFunctions.Init();
            TroopCompareFunction.Init();
            SkillSuccessMethod.Init();
            SkillCriticalMethod.Init();
            ModManager.Instance.Init();
            GameLanguage.Instance.Init("cn");

            StartCoroutine(GameInit());
        }
        public override void Shutdown()
        {
            MapRender.Instance.Clear();
#if SANGO_DEBUG
            Sango.Log.Print("游戏关闭");
#endif
            GameEvent.OnGameShutdown?.Invoke();
        }
        public override void Pause()
        {
#if SANGO_DEBUG
            Sango.Log.Print("游戏暂停");
#endif
            GameEvent.OnGamePause?.Invoke();
        }
        public override void Resume()
        {
#if SANGO_DEBUG
            Sango.Log.Print("游戏恢复");
#endif
            GameEvent.OnGameResume?.Invoke();
        }

        IEnumerator GameInit()
        {
            Window.Instance.Open("window_loading");
            //yield return new WaitForSeconds(0.5f);
            yield return null;
            ModManager.Instance.InitMods();
            GameData.Instance.Init();
            GameEvent.OnGameInit?.Invoke();
            GameState.Instance.ChangeState((int)GameState.State.GAME_START_MENU);
            Window.Instance.Open("window_start");
            Window.Instance.Close("window_loading");
            //Scenario scenario = new Scenario();
            //string path = Path.FindFile("Data/Scenario/Scenario.json");
            //scenario.FilePath = path;
            //scenario.CommonData = GameData.Instance.LoadCommonData();
            ////EnterMapEdior();
            //Scenario.Start(scenario);
            ////scenario.Save(Path.ContentRootPath + "/Save/Scenario.xml");
            Player.Player.Instance.Init();
        }

        public void EnterMapEditor()
        {
            Window.Instance.Close("window_start");
            GameObject map = new GameObject("map");
            MapEditor mapEditor = map.AddComponent<MapEditor>();
        }

        public void StartNewGame()
        {
            Window.Instance.Open("window_scenario_select");
            Window.Instance.Close("window_start");
        }

        public void StartGame(Scenario target)
        {

        }

        public override void Update()
        {
            GameController.Instance.Update();
            base.Update();
            if (Scenario.Cur != null && !Scenario.Cur.useThreadRun)
                Scenario.Cur.Run();
        }

        public static void DebugAI()
        {
            GameAIDebug.Enabled = !GameAIDebug.Enabled;
        }

    }
}
