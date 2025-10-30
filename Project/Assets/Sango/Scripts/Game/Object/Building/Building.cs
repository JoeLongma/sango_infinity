using Newtonsoft.Json;
using Sango.Game.Render;
using System.Collections.Generic;
using System.Text;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Building : BuildingBase
    {
        public override SangoObjectType ObjectType { get { return SangoObjectType.Building; } }

        public override string Name { get { return BuildingType?.Name; } }

        [JsonConverter(typeof(SangoObjectListIDConverter<Person>))]
        [JsonProperty]
        public SangoObjectList<Person> Builders { get; set; }

        //public Person Builder { get; set; }

        public Person Worker { get; set; }
        /// <summary>
        /// 占用槽位ID
        /// </summary>
        public int SlotId { get; set; }

        public int cellHarvestTotalFood = 0;
        public int cellHarvestTotalGold = 0;

        public override void OnScenarioPrepare(Scenario scenario)
        {
            base.OnScenarioPrepare(scenario);
            //Init(scenario);
        }

        public override void OnPrepareRender()
        {
            if (!BuildingType.isIntrior && Render == null)
                Render = new BuildingRender(this);
        }

        public override void Init(Scenario scenario)
        {
            BelongCity?.OnBuildingCreate(this);
            if (!BuildingType.isIntrior)
            {
                //UnityEngine.Debug.LogError($"{BelongCity.Name}-><{x},{y}>");
                CenterCell = scenario.Map.GetCell(x, y);
                CenterCell.building = this;
                effectCells = new System.Collections.Generic.List<Cell>();
                scenario.Map.GetDirectSpiral(CenterCell, BuildingType.radius, effectCells);
                //CalculateHarvest();

            }
            OnPrepareRender();
        }

        public override bool OnForceTurnStart(Scenario scenario)
        {
            if (!isComplte && Builders != null)
            {
                int totalValue = 0;
                for (int i = 0; i < Builders.Count; i++)
                {
                    Person person = Builders[i];
                    totalValue += person.BaseBuildAbility;
                }
                totalValue = GameUtility.Method_PersonBuildAbility(totalValue);
                durability += totalValue;
                if (durability >= BuildingType.durabilityLimit)
                {
                    durability = BuildingType.durabilityLimit;
                    isComplte = true;
                    //CalculateHarvest();
                    SangoObjectList<Person> builder = Builders;
                    OnBuildComplate();
                    BelongCity.OnBuildingComplete(this, builder);
                }
            }
            else if (isUpgrading && Builders != null)
            {
                int totalValue = 0;
                for (int i = 0; i < Builders.Count; i++)
                {
                    Person person = Builders[i];
                    totalValue += person.BaseBuildAbility;
                }
                totalValue = GameUtility.Method_PersonBuildAbility(totalValue);
                durability += totalValue;
                if (durability >= BuildingType.durabilityLimit)
                {
                    durability = BuildingType.durabilityLimit;
                    isUpgrading = false;
                    //CalculateHarvest();
                    SangoObjectList<Person> builder = Builders;
                    OnUpgradeComplate();
                    BelongCity.OnBuildingUpgradeComplete(this, builder);
                }

            }


            // 暂时写死
            if (isComplte && BuildingType.atkRange > 0)
            {
                List<Cell> atkCells = new List<Cell>();
                scenario.Map.GetSpiral(x, y, BuildingType.atkRange, atkCells);
                for (int i = 1; i < atkCells.Count; i++)
                {
                    Cell cell = atkCells[i];
                    if (cell.troop != null && IsEnemy(cell.troop))
                    {
                        BuildingAttackEvent @event = new BuildingAttackEvent()
                        {
                            building = this,
                            targetCell = cell,
                        };
                        RenderEvent.Instance.Add(@event);
                    }
                }
            }
            if (Render != null)
                Render.UpdateRender();
            return base.OnForceTurnStart(scenario);
        }

        public override void OnComplate(SangoObject builder)
        {
            Troop atk = builder as Troop;
            if (atk == null) return;

            Scenario scenario = Scenario.Cur;
            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Build;
            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            atk.ForEachPerson(person =>
            {
                person.merit += meritGain;
                person.GainExp(meritGain);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(" ");
#endif
            });

#if SANGO_DEBUG
            Sango.Log.Print($"[{BelongCity.Name}]{stringBuilder}完成{Name}建造!!");
#endif
            Tools.OverrideData<int> overrideData = new Tools.OverrideData<int>(techniquePointGain);
            GameEvent.OnCityJobGainTechniquePoint?.Invoke(BelongCity, jobId, Builders.objects.ToArray(), overrideData);
            techniquePointGain = overrideData.Value;

            BelongForce.GainTechniquePoint(techniquePointGain);
            Render.UpdateRender();

        }

        public virtual void OnBuildComplate()
        {
            Scenario scenario = Scenario.Cur;
            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Build;
            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            for (int i = 0; i < Builders.Count; i++)
            {
                Person person = Builders[i];
                person.merit += meritGain;
                person.GainExp(meritGain);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(" ");
#endif
                person.ClearMission();
            }
#if SANGO_DEBUG
            Sango.Log.Print($"[{BelongCity.Name}]{stringBuilder}完成{Name}建造!!");
#endif
            Tools.OverrideData<int> overrideData = new Tools.OverrideData<int>(techniquePointGain);
            GameEvent.OnCityJobGainTechniquePoint?.Invoke(BelongCity, jobId, Builders.objects.ToArray(), overrideData);
            techniquePointGain = overrideData.Value;

            BelongForce.GainTechniquePoint(techniquePointGain);

            Builders = null;
        }

        public void OnUpgradeComplate()
        {
            Scenario scenario = Scenario.Cur;
            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Build;
            int meritGain = variables.jobMaxPersonCount[jobId];
            int techniquePointGain = variables.jobTechniquePoint[jobId];

            BuildingType nextBuildingType = scenario.GetObject<BuildingType>(BuildingType.nextId);
            BuildingType = nextBuildingType;

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            for (int i = 0; i < Builders.Count; i++)
            {
                Person person = Builders[i];
                person.merit += meritGain;
                person.GainExp(meritGain);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(" ");
#endif
                person.ClearMission();
            }
#if SANGO_DEBUG
            Sango.Log.Print($"[{BelongCity.Name}]{stringBuilder}完成{Name}升级!!");
#endif
            Tools.OverrideData<int> overrideData = new Tools.OverrideData<int>(techniquePointGain);
            GameEvent.OnCityJobGainTechniquePoint?.Invoke(BelongCity, jobId, Builders.objects.ToArray(), overrideData);
            techniquePointGain = overrideData.Value;

            BelongForce.GainTechniquePoint(techniquePointGain);

            Builders = null;
        }

        public void ChangeCity(City dest)
        {
            if (!isComplte)
            {
                Sango.Log.Error("不允许转换一个未建好的建筑!!");
                return;
            }

            BelongCity.allOutterBuildings.Remove(this);

            dest.allOutterBuildings.Add(this);

            BelongCorps = dest.BelongCorps;
            BelongForce = dest.BelongForce;

            Render?.UpdateRender();
        }

        public Corps ChangeCorps(Corps corps)
        {
            Corps last = null;
            if (!isComplte)
            {
                Sango.Log.Error("不允许转换一个未建好的建筑!!");
                return last;
            }

            if (BelongCorps != corps)
            {
                last = BelongCorps;
                BelongCorps = corps;

                if (corps.BelongForce != BelongForce)
                {
                    BelongForce = corps.BelongForce;
                }

                Render?.UpdateRender();
            }
            return last;
        }

        public void Destroy()
        {
            Scenario.Cur.buildingSet.Remove(this);

            if (Builders != null)
            {
                for (int i = 0; i < Builders.Count; i++)
                {
                    Person person = Builders[i];
                    person.ClearMission();
                }
                Builders = null;
            }
            if (effectCells != null)
                effectCells.Clear();
            if (CenterCell != null)
            {
                CenterCell.building = null;
                CenterCell = null;
            }
            if (Render != null)
            {
                Render.Clear();
                Render = null;
            }
        }

        public override int GetSkillMethodAvaliabledTroops()
        {
            return 4000 + 4000 * durability / DurabilityLimit;
        }
        public override void OnFall(SangoObject atk)
        {
            BelongCity?.OnBuildingDestroy(this);
            Destroy();
        }
    }
}
