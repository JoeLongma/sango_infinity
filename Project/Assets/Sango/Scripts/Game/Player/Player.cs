using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sango.Game.Player
{
    public class Player : Singleton<Player>
    {
        public ShortScenario[] all_saved_scenario_list = new ShortScenario[50];

        public void Init()
        {
            //都市
            Singleton<CitySystem>.Instance.Init();

            // 建筑
            Singleton<BuildingSystem>.Instance.Init();

            // 部队
            Singleton<TroopSystem>.Instance.Init();

            // 其他
            Singleton<GameSettingSystem>.Instance.Init();

            InitSaveFile();
        }

        void InitSaveFile()
        {
            for (int i = 1; i <= all_saved_scenario_list.Length; i++)
            {
                string fileName = $"{Path.SaveRootPath}/Save/save{i}.json";
                if (File.Exists(fileName))
                {
#if SANGO_DEBUG
                    Sango.Log.Print($"Find Saved data : {fileName}");
#endif
                    ShortScenario scenario = new ShortScenario(fileName);
                    all_saved_scenario_list[i - 1] = scenario;
                }
            }
        }

        public void Save(int index)
        {
            string fileName = $"{Path.SaveRootPath}/Save/save{index+1}.json";
            Scenario.Cur.Save(fileName);
            all_saved_scenario_list[index] = new ShortScenario(fileName);
            GameEvent.OnGameSave?.Invoke(Scenario.Cur, index);
        }

        public void Load(int index)
        {
            Window.Instance.CloseAll();
            Window.Instance.Open("window_loading");
            Quit();
            string fileName = $"{Path.SaveRootPath}/Save/save{index+1}.json";
            Scenario.CurSelected = new Scenario(fileName);
            Scenario.StartScenario(Scenario.CurSelected);
        }

        public void Quit()
        {
            Scenario.Cur?.OnGameShutdown();
        }

        public void QuitToMainMenu()
        {
            Scenario.Cur?.OnGameShutdown();
            Window.Instance.CloseAll();
            Window.Instance.DestroyAll();
            Window.Instance.Open("window_start");

        }
    }
}
