using Sango.Game;
using Sango.Game.Render.UI;
using Sango.Game.Tools;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sango.Game
{
    public class GameEvent : EventBase
    {

        #region Global
        /// <summary>
        /// 游戏状态监听 -Enter
        /// </summary>
        public static EventDelegate<int, int> OnGameStateEnter;

        /// <summary>
        /// 游戏状态监听 -Exit
        /// </summary>
        public static EventDelegate<int, int> OnGameStateExit;
        #endregion Global

        #region Scenario
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


        #region Window
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
        public static EventDelegate<ContextMenuData> OnContextMenuShow;

        public static EventDelegate<ContextMenuData, BuildingBase> OnBuildingContextMenuShow;
        public static EventDelegate<ContextMenuData, City> OnCityContextMenuShow;
        public static EventDelegate<ContextMenuData, Troop> OnTroopContextMenuShow;

        #endregion Window

        #region Game
        /// <summary>
        /// 新天开始
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
        /// 新季节开始
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

        /// <summary>
        /// 势力逻辑开始
        /// </summary>
        public static EventDelegate<Force, Scenario> OnForceStart;

        /// <summary>
        /// 势力逻辑结束
        /// </summary>
        public static EventDelegate<Force, Scenario> OnForceEnd;

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
        /// 城池AI
        /// </summary>
        public static EventDelegate<City, Scenario> OnCityAIPrepare;
        public static EventDelegate<City, Scenario> OnCityAIStart;
        public static EventDelegate<City, Scenario> OnCityAIEnd;

        /// <summary>
        /// 部队AI
        /// </summary>
        public static EventDelegate<Troop, Scenario> OnTroopAIStart;
        public static EventDelegate<Troop, Scenario> OnTroopAIPrepare;
        public static EventDelegate<Troop, Scenario> OnTroopAIEnd;


        public static EventDelegate<Cell, Cell> OnTroopLeaveCell;
        public static EventDelegate<Cell, Cell> OnTroopEnterCell;

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
        public static EventDelegate<Troop, Troop, Skill, Scenario, OverrideData<int>> OnTroopCalculateAttackBack;

        /// <summary>
        /// 当城池沦陷的时候
        /// </summary>
        public static EventDelegate<City, Troop> OnCityFall;

        /// <summary>
        /// 可监听改写工作花费
        /// City, JobType, PersonList, PersonCount, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, int, Person[], OverrideData<int>> OnCityCheckJobCost;

        /// <summary>
        /// 可监听改写工作成果
        /// City, JobType, PersonList, PersonCount, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, int, Person[], OverrideData<int>> OnCityJobResult;

        /// <summary>
        /// 可监听改写工作获取的技巧
        /// City, JobType, PersonList, PersonCount, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, int, Person[], OverrideData<int>> OnCityJobGainTechniquePoint;

        /// <summary>
        /// 可监听改写发现人才的几率
        /// City, JobType, PersonList, PersonCount, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, int, Person, OverrideData<int>> OnCityJobSearchingWild;

        /// <summary>
        /// 可监听改计算城池最大士气
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxMorale;

        /// <summary>
        /// 可监听改计算城池最大资金
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxGold;

        /// <summary>
        /// 可监听改计算城池最大兵粮
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxFood;

        /// <summary>
        /// 可监听改计算城池最大仓库数量
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxItemStoreSize;

        /// <summary>
        /// 可监听改计算城池最大士兵数
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxTroops;

        /// <summary>
        /// 可监听改计算城池最大耐久
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCityCalculateMaxDurability;

        /// <summary>
        /// 可监听改计算部队最大兵力
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, Troop, OverrideData<int>> OnTroopCalculateMaxTroops;

        /// <summary>
        /// 可监听改计算城池每季度治安下降值
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<City, OverrideData<int>> OnCitySecurityChangeOnSeasonStart;

        /// <summary>
        /// 可监听改计算建筑反击攻击力
        /// City, Troop, lastValue, OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, Cell, BuildingBase, Skill, OverrideData<int>> OnBuildCalculateAttackBack;

        /// <summary>
        /// 武将升级事件
        /// </summary>
        public static EventDelegate<Person> OnPersonLevelUp;

        /// <summary>
        /// 势力忠诚换季衰减概率
        /// </summary>
        public static EventDelegate<Force, OverrideData<int>> OnForcePersonLoyaltyChangeProbability;

        /// <summary>
        /// 可监听改计算战法成功率(百分比)
        /// City, Skill, spellCell, OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, Skill, Cell, OverrideData<int>> OnTroopCalculateSkillSuccess;

        /// <summary>
        /// 可监听改计算战法暴击率(百分比)
        /// City, Skill, spellCell,  OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, Skill, Cell, OverrideData<int>> OnTroopCalculateSkillCritical;

        /// <summary>
        /// 可监听改计算战法暴击时的伤害倍率(百分比)
        /// City, Skill, spellCell,  OverrideFunc
        /// </summary>
        public static EventDelegate<Troop, Skill, Cell, OverrideData<int>> OnTroopCalculateSkillCriticalFactor;


        /// <summary>
        /// 当部队兵力变化时
        /// </summary>
        public static EventDelegate<Troop, SangoObject, Skill, int, OverrideData<int>> OnTroopChangeTroops;

        #endregion Game

        #region Override

        #endregion
    }
}
