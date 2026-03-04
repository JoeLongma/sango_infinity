using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 剧本选择界面
    /// </summary>
    public class UIScenarioVariables : UGUIWindow, IVariablesSetting
    {
        public Text title;

        public GameObject titleObj;
        public GameObject bigTitleObj;
        public GameObject integerObj;
        public GameObject floatObj;
        public GameObject integerSliderObj;
        public GameObject floatSliderObj;
        public GameObject dropdownObj;
        public GameObject toggleObj;
        public GameObject toggleGroupObj;

        List<GameObject> pool_titleObj = new List<GameObject>();
        List<GameObject> pool_bigTitleObj = new List<GameObject>();
        List<GameObject> pool_integerObj = new List<GameObject>();
        List<GameObject> pool_floatObj = new List<GameObject>();
        List<GameObject> pool_integerSliderObj = new List<GameObject>();
        List<GameObject> pool_floatSliderObj = new List<GameObject>();
        List<GameObject> pool_dropdownObj = new List<GameObject>();
        List<GameObject> pool_toggleObj = new List<GameObject>();
        List<GameObject> pool_toggleGroupObj = new List<GameObject>();

        public override void OnShow()
        {
            for (int i = 0; i < itemList.Count; i++)
                RemoveItem(itemList[i]);
            itemList.Clear();
            Scenario.CurSelected.LoadVariables();
            ShowVariables(Scenario.CurSelected);
        }

        public void ShowVariables(Scenario scenario)
        {
            ScenarioVariables variables = scenario.Variables;
            AddBigTitle(scenario.Name);
            AddTitle("难度选择");
            AddToggleGroupItem("难度", variables.difficulty, new List<string> { "简单", "普通", "困难", "超级" }, (v) => { variables.difficulty = v; });
            AddTitle("剧本基础参数");
            AddNumberItem("行动力上限", variables.ActionPointLimit, 0, 10000, (v) => { variables.ActionPointLimit = v; });
            AddNumberItem("行动力获取倍率", variables.ActionPointFactor, 0, 100, (v) => { variables.ActionPointFactor = v; });
            AddToggleItem("年龄生效", variables.AgeEnabled, (v) => { variables.AgeEnabled = v; RefreshSetting(); });
            if (variables.AgeEnabled)
                AddToggleItem("能力随年龄变化", variables.EnableAgeAbilityFactor, (v) => { variables.EnableAgeAbilityFactor = v; });
            AddToggleItem("人口系统开关", variables.populationEnable, (v) => { variables.populationEnable = v; });
            AddNumberItem("基础人口增长率", variables.populationIncreaseBaseFactor, 0, 1, (v) => { variables.populationIncreaseBaseFactor = v; });
            AddNumberItem("队伍粮食基础消耗率", variables.baseFoodCostInTroop, 0, 100, (v) => { variables.baseFoodCostInTroop = v; });
            AddNumberItem("城池中粮食基础消耗率", variables.baseFoodCostInCity, 0, 100, (v) => { variables.baseFoodCostInCity = v; });
            AddNumberItem("治安对征兵的影响值比例", variables.securityInfluenceRecruitTroops, 0, 1, (v) => { variables.securityInfluenceRecruitTroops = v; });
            AddNumberItem("建筑建造最大回合数", variables.BuildMaxTurn, 0, 100, (v) => { variables.BuildMaxTurn = v; });
            AddNumberItem("粮食倍率", variables.foodFactor, 0, 100, (v) => { variables.foodFactor = v; });
            AddNumberItem("金币倍率", variables.goldFactor, 0, 100, (v) => { variables.goldFactor = v; });
            AddNumberItem("每月变化的关系值", variables.relationChangePerMonth, 0, 100, (v) => { variables.relationChangePerMonth = v; });
            AddNumberItem("每月的关系变化率(百分比)", variables.relationChangeChance, 0, 100, (v) => { variables.relationChangeChance = v; });
            AddNumberItem("破城时候的抓捕率(百分比)", variables.captureChangceWhenCityFall, 0, 100, (v) => { variables.captureChangceWhenCityFall = v; });
            AddNumberItem("最后一城时候的抓捕率(百分比)", variables.captureChangceWhenLastCityFall, 0, 100, (v) => { variables.captureChangceWhenLastCityFall = v; });

            AddTitle("部队参数");
            AddNumberItem("攻击-武力影响(万分比)", variables.fight_troop_attack_strength_factor, 0, 100000, (v) => { variables.fight_troop_attack_strength_factor = v; });
            AddNumberItem("攻击-智力影响(万分比)", variables.fight_troop_attack_intelligence_factor, 0, 100000, (v) => { variables.fight_troop_attack_intelligence_factor = v; });
            AddNumberItem("攻击-统率影响(万分比)", variables.fight_troop_attack_command_factor, 0, 100000, (v) => { variables.fight_troop_attack_command_factor = v; });
            AddNumberItem("攻击-政治影响(万分比)", variables.fight_troop_attack_politics_factor, 0, 100000, (v) => { variables.fight_troop_attack_politics_factor = v; });
            AddNumberItem("攻击-魅力影响(万分比)", variables.fight_troop_attack_glamour_factor, 0, 100000, (v) => { variables.fight_troop_attack_glamour_factor = v; });

            AddNumberItem("防御-武力影响(万分比)", variables.fight_troop_defence_strength_factor, 0, 100000, (v) => { variables.fight_troop_defence_strength_factor = v; });
            AddNumberItem("防御-智力影响(万分比)", variables.fight_troop_defence_intelligence_factor, 0, 100000, (v) => { variables.fight_troop_defence_intelligence_factor = v; });
            AddNumberItem("防御-统率影响(万分比)", variables.fight_troop_defence_command_factor, 0, 100000, (v) => { variables.fight_troop_defence_command_factor = v; });
            AddNumberItem("防御-政治影响(万分比)", variables.fight_troop_defence_politics_factor, 0, 100000, (v) => { variables.fight_troop_defence_politics_factor = v; });
            AddNumberItem("防御-魅力影响(万分比)", variables.fight_troop_defence_glamour_factor, 0, 100000, (v) => { variables.fight_troop_defence_glamour_factor = v; });

            AddNumberItem("近战击溃部队缴获的金钱比例(百分比)", variables.defeatTroopCanGainGoldFactor, 0, 100, (v) => { variables.defeatTroopCanGainGoldFactor = v; });
            AddNumberItem("近战击溃部队缴获的粮食比例(百分比)", variables.defeatTroopCanGainFoodFactor, 0, 100, (v) => { variables.defeatTroopCanGainFoodFactor = v; });


            AddTitle("AI参数");
            AddNumberItem("进攻时候留守最低的兵力", variables.minTroopsKeepWhenAttack, 0, 100, (v) => { variables.minTroopsKeepWhenAttack = v; });
            AddNumberItem("进攻时候留守最低的粮食", variables.minFoodKeepWhenAttack, 0, 100, (v) => { variables.minFoodKeepWhenAttack = v; });
            AddNumberItem("防御时候留守最低的兵力", variables.minTroopsKeepWhenDefence, 0, 100, (v) => { variables.minTroopsKeepWhenDefence = v; });
            AddNumberItem("防御时候留守最低的粮食", variables.minFoodKeepWhenDefence, 0, 100, (v) => { variables.minFoodKeepWhenDefence = v; });

            AddBigTitle("Mod相关");
            GameEvent.OnScenarioVariablesSetting?.Invoke(this, scenario);
        }

        List<GameObject> itemList = new List<GameObject>();

        public void RefreshSetting()
        {
            for (int i = 0; i < itemList.Count; i++)
                RemoveItem(itemList[i]);
            itemList.Clear();
            ShowVariables(Scenario.CurSelected);
        }

        public void RemoveItem(GameObject item)
        {
            string name = item.name;
            switch (name)
            {
                case "titleObj":
                    pool_titleObj.Add(item);
                    break;
                case "bigTitleObj":
                    pool_bigTitleObj.Add(item);
                    break;
                case "integerObj":
                    pool_integerObj.Add(item);
                    break;
                case "floatObj":
                    pool_floatObj.Add(item);
                    break;
                case "integerSliderObj":
                    pool_integerSliderObj.Add(item);
                    break;
                case "floatSliderObj":
                    pool_floatSliderObj.Add(item);
                    break;
                case "dropdownObj":
                    pool_dropdownObj.Add(item);
                    break;
                case "toggleObj":
                    pool_toggleObj.Add(item);
                    break;
                case "toggleGroupObj":
                    pool_toggleGroupObj.Add(item);
                    break;
            }
            item.SetActive(false);
        }

        public void SetItemBehindThis(GameObject item, GameObject t)
        {
            item.transform.SetSiblingIndex(t.transform.GetSiblingIndex() + 1);
        }

        public GameObject AddBigTitle(string title)
        {
            GameObject obj;
            if (pool_bigTitleObj.Count > 0)
            {
                obj = pool_bigTitleObj[0];
                pool_bigTitleObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(bigTitleObj, bigTitleObj.transform.parent);
                obj.name = "bigTitleObj";
            }
            itemList.Add(obj);
            obj.GetComponent<UITitleField>().Set(title);
            obj.SetActive(true);
            return obj;
        }

        public GameObject AddTitle(string title)
        {
            GameObject obj;
            if (pool_titleObj.Count > 0)
            {
                obj = pool_titleObj[0];
                pool_titleObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(titleObj, titleObj.transform.parent);
                obj.name = "titleObj";
            }
            itemList.Add(obj);
            obj.GetComponent<UITitleField>().Set(title);
            obj.SetActive(true);
            return obj;
        }


        public GameObject AddNumberItem(string title, int number, int min, int max, System.Action<int> onChange)
        {
            GameObject obj;
            if (pool_integerObj.Count > 0)
            {
                obj = pool_integerObj[0];
                pool_integerObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(integerObj, integerObj.transform.parent);
                obj.name = "integerObj";
            }

            itemList.Add(obj);
            obj.GetComponent<UIIntegerField>().Set(title, number, min, max, onChange);
            obj.SetActive(true);
            return obj;
        }

        public GameObject AddNumberItem(string title, float number, float min, float max, System.Action<float> onChange)
        {
            GameObject obj;
            if (pool_floatObj.Count > 0)
            {
                obj = pool_floatObj[0];
                pool_floatObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(floatObj, floatObj.transform.parent);
                obj.name = "floatObj";
            }
            itemList.Add(obj);
            obj.GetComponent<UIFloatField>().Set(title, number, min, max, onChange);
            obj.SetActive(true);
            return obj;
        }

        public GameObject AddSliderItem(string title, int value, int min, int max, System.Action<int> onValueChange)
        {
            GameObject obj;
            if (pool_integerSliderObj.Count > 0)
            {
                obj = pool_integerSliderObj[0];
                pool_integerSliderObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(integerSliderObj, integerSliderObj.transform.parent);
                obj.name = "integerSliderObj";
            }
            itemList.Add(obj);
            obj.GetComponent<UIIntegerSliderField>().Set(title, value, min, max, onValueChange);
            obj.SetActive(true);
            return obj;
        }

        public GameObject AddSliderItem(string title, float value, float min, float max, System.Action<float> onValueChange)
        {
            GameObject obj;
            if (pool_floatSliderObj.Count > 0)
            {
                obj = pool_floatSliderObj[0];
                pool_floatSliderObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(floatSliderObj, floatSliderObj.transform.parent);
                obj.name = "floatSliderObj";
            }
            itemList.Add(obj);
            obj.GetComponent<UIFloatSliderField>().Set(title, value, min, max, onValueChange);
            obj.SetActive(true);
            return obj;
        }

        public GameObject AddToggleItem(string title, bool value, System.Action<bool> onValueChange)
        {
            GameObject obj;
            if (pool_toggleObj.Count > 0)
            {
                obj = pool_toggleObj[0];
                pool_toggleObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(toggleObj, toggleObj.transform.parent);
                obj.name = "toggleObj";
            }
            itemList.Add(obj);
            obj.GetComponent<UIToggleField>().Set(title, value, onValueChange);
            obj.SetActive(true);
            return obj;
        }

        public GameObject AddDropdownItem(string title, int value, List<string> values, System.Action<int> onValueChange)
        {
            GameObject obj;
            if (pool_dropdownObj.Count > 0)
            {
                obj = pool_dropdownObj[0];
                pool_dropdownObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(dropdownObj, dropdownObj.transform.parent);
                obj.name = "dropdownObj";
            }
            itemList.Add(obj);
            obj.GetComponent<UIDropdownField>().Set(title, value, values, onValueChange);
            obj.SetActive(true);
            return obj;
        }

        public GameObject AddToggleGroupItem(string title, int value, List<string> values, System.Action<int> onValueChange)
        {
            GameObject obj;
            if (pool_toggleGroupObj.Count > 0)
            {
                obj = pool_toggleGroupObj[0];
                pool_toggleGroupObj.RemoveAt(0);
            }
            else
            {
                obj = GameObject.Instantiate(toggleGroupObj, toggleGroupObj.transform.parent);
                obj.name = "toggleGroupObj";
            }
            itemList.Add(obj);
            obj.GetComponent<UIToggleGroupField>().Set(title, value, values, onValueChange);
            obj.SetActive(true);
            return obj;
        }

        public void OnStartGame()
        {
            Window.Instance.Open("window_loading");
            Window.Instance.Close("window_scenario_variables");
            Scenario.StartScenario(Scenario.CurSelected);
        }

        public void OnCancel()
        {
            Window.Instance.Close("window_scenario_variables");
            Window.Instance.Open("window_scenario_force_select");
        }
    }
}
