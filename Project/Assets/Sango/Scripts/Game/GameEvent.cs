using Sango.Game.Tools;
using UnityEngine;

namespace Sango.Game
{
    public class GameEvent : EventBase
    {
        #region Global 全局变量
        /// <summary>
        /// 游戏状态监听 -Enter
        /// </summary>
        public static EventDelegate<int, int> OnGameStateEnter;

        /// <summary>
        /// 游戏状态监听 -Exit
        /// </summary>
        public static EventDelegate<int, int> OnGameStateExit;

        /// <summary>
        /// 可扩展适应名字
        /// </summary>
        public static EventDelegateReturn<string, int> OnGetAbilityName;

        /// <summary>
        /// 可扩展获取武将属性名字
        /// </summary>
        public static EventDelegateReturn<string, int> OnGetAttributeName;
        public static EventDelegateReturn<string, int> OnGetAttributeNameWithColor;

        public static EventDelegate<Cell, Vector3, bool> OnClick;
        public static EventDelegate<Cell, Vector3, bool> OnRClick;
        public static EventDelegate<Cell, Vector3, bool> OnCancel;

        /// <summary>
        /// 在最后一个系统出栈时候
        /// </summary>
        public static EventDelegate OnSystemEnd;

        /// <summary>
        /// 在第一个系统入栈时
        /// </summary>
        public static EventDelegate OnSystemStart;

        #endregion Global

        #region Scenario 剧本
        /// <summary>
        /// 剧本加载开始
        /// </summary>
        public static EventDelegate<Scenario> OnScenarioLoadStart;

        /// <summary>
        /// 剧本加载结束
        /// </summary>
        public static EventDelegate<Scenario> OnScenarioLoadEnd;

        /// <summary>
        /// 地图场景加载开始
        /// </summary>
        public static EventDelegate<Scenario> OnWorldLoadStart;

        /// <summary>
        /// 地图场景加载结束
        /// </summary>
        public static EventDelegate<Scenario> OnWorldLoadEnd;

        /// <summary>
        /// 剧本开始
        /// </summary>
        public static EventDelegate<Scenario> OnScenarioStart;

        /// <summary>
        /// 剧本准备
        /// </summary>
        public static EventDelegate<Scenario> OnScenarioPrepare;

        /// <summary>
        /// 剧本准备
        /// </summary>
        public static EventDelegate<Scenario> OnScenarioInit;

        /// <summary>
        /// 剧本结束
        /// </summary>
        public static EventDelegate<Scenario> OnScenarioEnd;

        /// <summary>
        /// 剧本tick
        /// </summary>
        public static EventDelegate<Scenario, float> OnScenarioTick;
        #endregion Scenario

        #region Window 窗体
        /// <summary>
        /// 在window创建之后
        /// </summary>
        public static EventDelegate<string, Window.WindowInterface> OnWindowCreate;

        /// <summary>
        ///  临时-城市扩展信息
        /// </summary>
        public static EventDelegate OnCityHeadbarShowInfoChange;

        /// <summary>
        /// ContextMenu激活的时候,可以监听来添加自定义按钮
        /// </summary>
        public static EventDelegate<IContextMenuData> OnContextMenuShow;

        public static EventDelegate<IContextMenuData, BuildingBase> OnBuildingContextMenuShow;
        public static EventDelegate<IContextMenuData, City> OnCityContextMenuShow;
        public static EventDelegate<IContextMenuData, Troop> OnTroopContextMenuShow;
        public static EventDelegate<IContextMenuData, Troop, Cell> OnTroopActionContextMenuShow;
        public static EventDelegate<IContextMenuData> OnRightMouseButtonContextMenuShow;

        public static EventDelegate<IContextMenuData> OnGameSettingContextMenuShow;

        public static EventDelegate<ITroopInteractiveDialogData, Troop, Cell> OnTroopInteractiveContextDialogShow;
        public static EventDelegate<IVariablesSetting, Scenario> OnScenarioVariablesSetting;
        #endregion Window

        #region Game 游戏

        /// <summary>
        /// 游戏保存
        /// </summary>
        public static EventDelegate<Scenario, int> OnGameSave;

        /// <summary>
        /// 游戏加载
        /// </summary>
        public static EventDelegate<Scenario> OnGameLoad;

        /// <summary>
        /// 新日开始
        /// </summary>
        public static EventDelegate<Scenario> OnDayUpdate;

        /// <summary>
        /// 新月开始
        /// </summary>
        public static EventDelegate<Scenario> OnMonthUpdate;

        /// <summary>
        /// 新年开始
        /// </summary>
        public static EventDelegate<Scenario> OnYearUpdate;

        /// <summary>
        /// 新季开始
        /// </summary>
        public static EventDelegate<Scenario> OnSeasonUpdate;

        /// <summary>
        /// 回合开始
        /// </summary>
        public static EventDelegate<Scenario> OnTurnStart;

        /// <summary>
        /// 回合结束
        /// </summary>
        public static EventDelegate<Scenario> OnTurnEnd;

        #region Force
        /// <summary>
        /// 势力逻辑开始
        /// </summary>
        public static EventDelegate<Force, Scenario> OnForceTurnStart;

        /// <summary>
        /// 势力逻辑结束
        /// </summary>
        public static EventDelegate<Force, Scenario> OnForceTurnEnd;

        /// <summary>
        /// 玩家控制势力
        /// </summary>
        public static EventDelegate<Corps, Scenario> OnPlayerControl;

        /// <summary>
        /// 势力AI
        /// </summary>
        public static EventDelegate<Force, Scenario> OnForceAIPrepare;
        public static EventDelegate<Force, Scenario> OnForceAIStart;
        public static EventDelegate<Force, Scenario> OnForceAIEnd;

        /// <summary>
        /// 当势力灭亡的时候
        /// </summary>
        public static EventDelegate<Force, City, Troop> OnForceFall;
        public static EventDelegate<Force, int> OnForceGainTechniquePoint;
        public static EventDelegate<Force, Technique> OnForceResearchComplete;

        #endregion Force

        /// <summary>
        /// 城池AI
        /// </summary>
        public static EventDelegate<City, Scenario> OnCityAIPrepare;
        public static EventDelegate<City, Scenario> OnCityAIStart;
        public static EventDelegate<City, Scenario> OnCityAIEnd;

        public static EventDelegate<City, Scenario> OnCityTurnStart;
        public static EventDelegate<City, Scenario> OnCityTurnEnd;
        public static EventDelegate<City, Scenario> OnCityMonthStart;
        public static EventDelegate<City, Scenario> OnCitySeasonStart;

        /// <summary>
        /// 部队AI
        /// </summary>
        public static EventDelegate<Troop, Scenario> OnTroopAIStart;
        public static EventDelegate<Troop, Scenario> OnTroopAIPrepare;
        public static EventDelegate<Troop, Scenario> OnTroopAIEnd;

        public static EventDelegate<Troop, Cell, Cell> OnTroopLeaveCell;
        public static EventDelegate<Troop, Cell, Cell> OnTroopEnterCell;

        public static EventDelegate<Troop, Scenario> OnTroopTurnStart;
        public static EventDelegate<Troop, Scenario> OnTroopTurnEnd;

        public static EventDelegate<Building, Scenario> OnBuildingTurnStart;
        public static EventDelegate<Building, Scenario> OnBuildingTurnEnd;

        /// <summary>
        /// 部队组建的时候
        /// </summary>
        public static EventDelegate<Troop, Scenario> OnTroopCreated;

        /// <summary>
        /// 部队溃灭的时候
        /// </summary>
        public static EventDelegate<Troop, Scenario> OnTroopDestroyed;

        /// <summary>
        /// 部队计算属性的时候
        /// </summary>
        public static EventDelegate<Troop, Scenario> OnTroopCalculateAttribute;

        /// <summary>
        /// 部队计算反击的时候
        /// </summary>
        public static EventDelegate<Troop, Troop, SkillInstance, Scenario, OverrideData<int>> OnTroopCalculateAttackBack;

        /// <summary>
        /// 当城池沦陷的时候
        /// </summary>
        public static EventDelegate<City, Force, Troop> OnCityFall;

        /// <summary>
        /// 可监听改写工作花费
        /// City, JobType, PersonList, OverrideData
        /// </summary>
        public static EventDelegate<City, int, Person[], OverrideData<int>> OnCityCheckJobCost;

        /// <summary>
        /// 可监听改写工作成果
        /// City, JobType, PersonList, OverrideData
        /// </summary>
        public static EventDelegate<City, int, Person[], OverrideData<int>> OnCityJobResult;

        /// <summary>
        /// 可监听改写工作回合
        /// City, JobType, PersonList, OverrideData
        /// </summary>
        public static EventDelegate<City, int, Person[], OverrideData<int>> OnCityJobCounterResult;

        /// <summary>
        /// 可监听改写工作获取的技巧
        /// City, JobType, PersonList, OverrideData
        /// </summary>
        public static EventDelegate<City, int, Person[], OverrideData<int>> OnCityJobGainTechniquePoint;

        /// <summary>
        /// 可监听改写工作获取的经验
        /// City, JobType, PersonList, OverrideData
        /// </summary>
        public static EventDelegate<City, int, Person[], OverrideData<int>> OnCityJobGainMerit;

        /// <summary>
        /// 可监听改写发现人才的几率
        /// City, JobType, PersonList, OverrideData
        /// </summary>
        public static EventDelegate<City, int, Person, OverrideData<int>> OnCityJobSearchingWild;

        /// <summary>
        /// 可监听改计算城池最大士气
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxMorale;

        /// <summary>
        /// 可监听改计算城池最大资金
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxGold;

        /// <summary>
        /// 可监听改计算城池最大兵粮
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxFood;

        /// <summary>
        /// 可监听改计算城池最大仓库数量
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxItemStoreSize;

        /// <summary>
        /// 可监听改计算城池最大士兵数
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxTroops;

        /// <summary>
        /// 可监听改计算城池最大耐久
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxDurability;

        /// <summary>
        /// 可监听改计算部队最大兵力
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<City, Troop, OverrideData<int>> OnTroopCalculateMaxTroops;

        /// <summary>
        /// 可监听改计算城池每季度治安下降值
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCitySecurityChangeOnSeasonStart;

        /// <summary>
        /// 可监听改写研究特定技巧的花费和时间
        /// 城市, 执行者, 科技, 技巧点花费, 资金花费, 时间
        /// </summary>
        public static EventDelegate<City, Person[], Technique, OverrideData<int>, OverrideData<int>, OverrideData<int>> OnCityResearchCost;

        /// <summary>
        /// 可监听改计算建筑兵粮产出
        /// City, Troop, OverrideData
        /// </summary>
        public static EventDelegate<BuildingBase, OverrideData<int>> OnBuildingCalculateFoodGain;

        /// <summary>
        /// 可监听改计算建筑资金产出
        /// BuildingBase, OverrideData
        /// </summary>
        public static EventDelegate<BuildingBase, OverrideData<int>> OnBuildingCalculateGoldGain;

        /// <summary>
        /// 可监听改计算建筑人口增长率
        /// BuildingBase, OverrideData
        /// </summary>
        public static EventDelegate<BuildingBase, OverrideData<int>> OnBuildingCalculatePopulationGain;

        /// <summary>
        /// 可监听改计算建筑人口增长率
        /// BuildingBase, OverrideData
        /// </summary>
        public static EventDelegate<BuildingBase, OverrideData<int>> OnBuildingCalculateProduct;

        /// <summary>
        /// 可监听改计算建筑反击攻击力
        /// Troop, Cell, BuildingBase, Skill, OverrideData
        /// </summary>
        public static EventDelegate<Troop, Cell, BuildingBase, SkillInstance, OverrideData<int>> OnBuildCalculateAttackBack;

        /// <summary>
        /// 武将升级事件
        /// </summary>
        public static EventDelegate<Person> OnPersonLevelUp;

        /// <summary>
        /// 势力忠诚换季衰减概率
        /// </summary>
        public static EventDelegate<Force, OverrideData<int>> OnForcePersonLoyaltyChangeProbability;

        /// <summary>
        /// 可监听改计算战法成功率(百分比) 必爆, 设置100则为必中
        /// City, Skill, spellCell, OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, SkillInstance, Cell, OverrideData<int>> OnTroopBeforeCalculateSkillSuccess;

        /// <summary>
        /// 
        /// 可监听改计算战法成功率(百分比)
        /// City, Skill, spellCell, OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, SkillInstance, Cell, OverrideData<int>> OnTroopAfterCalculateSkillSuccess;

        /// <summary>
        /// 可监听改计算战法暴击率(百分比) 必爆, 设置100则为必爆
        /// City, Skill, spellCell,  OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, SkillInstance, Cell, OverrideData<int>> OnTroopBeforeCalculateSkillCritical;

        /// <summary>
        /// 可监听改计算战法暴击率(百分比)
        /// City, Skill, spellCell,  OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, SkillInstance, Cell, OverrideData<int>> OnTroopAfterCalculateSkillCritical;

        /// <summary>
        /// 可监听改计算战法暴击时的伤害倍率(百分比)
        /// City, Skill, spellCell,  OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, SkillInstance, Cell, OverrideData<int>> OnTroopCalculateSkillCriticalFactor;

        /// <summary>
        /// 当部队兵力变化时
        /// </summary>
        public static EventDelegate<Troop, SangoObject, SkillInstance, int, OverrideData<int>> OnTroopChangeTroops;

        /// <summary>
        /// 当部队兵力变化时
        /// </summary>
        public static EventDelegate<Corps> OnCorpsActionPointChange;

        /// <summary>
        /// 当部队结束行动时
        /// </summary>
        public static EventDelegate<Troop> OnTroopActionOver;

        /// <summary>
        /// 技能实例计算属性时
        /// </summary>
        public static EventDelegate<Troop, SkillInstance> OnSkillCalculateAttribute;

        /// <summary>
        /// 当武将逃跑时
        /// </summary>
        public static EventDelegate<Person, SangoObject> OnPersonEscape;

        #endregion Game
    }
}
