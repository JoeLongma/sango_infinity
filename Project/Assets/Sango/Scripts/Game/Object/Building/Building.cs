using Newtonsoft.Json;
using Sango.Game.Action;
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
        public SangoObjectList<Person> Workers { get; set; }

        [JsonConverter(typeof(SangoObjectListIDConverter<Person>))]
        [JsonProperty]
        public SangoObjectList<Person> Builder { get; set; }

        [JsonProperty]
        public int LeftCounter { get; set; }

        [JsonProperty]
        public int ProductItemId { get; set; }

        [JsonProperty]
        public int AccumulatedProduct { get; set; }

        [JsonProperty]
        public int AccumulatedFood { get; set; }

        [JsonProperty]
        public int AccumulatedGold { get; set; }

        [JsonProperty]
        public int AccumulatedPopulation { get; set; }

        public int cellHarvestTotalFood = 0;
        public int cellHarvestTotalGold = 0;
        public List<ActionBase> actionList;

        public override void OnScenarioPrepare(Scenario scenario)
        {
            base.OnScenarioPrepare(scenario);
            //Init(scenario);
        }

        public override void OnPrepareRender()
        {
            if (Render == null)
                Render = new BuildingRender(this);
        }

        public override void Init(Scenario scenario)
        {
            BelongCity?.OnBuildingCreate(this);
            // 地格占用
            OccupyCellList = new List<Cell>();
            scenario.Map.GetSpiral(x, y, BuildingType.radius, OccupyCellList);
            foreach (Cell cell in OccupyCellList)
                cell.building = this;
            CenterCell = OccupyCellList[0];

            if (CenterCell.IsInterior)
                CenterCell.ClearInteriorModel();

            actionList = new List<ActionBase>();
            BuildingType.InitActions(actionList, this);

            // 效果范围
            effectCells = new System.Collections.Generic.List<Cell>();
            scenario.Map.GetDirectSpiral(CenterCell, BuildingType.radius + 1, BuildingType.radius + BuildingType.atkRange, effectCells);
            OnPrepareRender();
        }

        public override bool OnForceTurnStart(Scenario scenario)
        {
            ActionOver = false;


            if (!isComplate && Builder != null)
            {
                int totalValue = (BuildingType.durabilityLimit - durability) / LeftCounter;
                durability += totalValue;
                if (durability >= BuildingType.durabilityLimit)
                {
                    durability = BuildingType.durabilityLimit;
                    isComplate = true;
                    //CalculateHarvest();
                    SangoObjectList<Person> builder = Builder;
                    OnBuildComplate();
                    BelongCity.OnBuildingComplete(this, builder);
                }
                if (LeftCounter > 0)
                    LeftCounter--;
            }
            else if (isUpgrading && Builder != null)
            {
                if (LeftCounter > 0)
                    LeftCounter--;
                int totalValue = GameUtility.Method_PersonBuildAbility(Builder);
                durability += totalValue;
                if (durability >= BuildingType.durabilityLimit)
                {
                    durability = BuildingType.durabilityLimit;
                    isUpgrading = false;
                    //CalculateHarvest();
                    SangoObjectList<Person> builder = Builder;
                    OnUpgradeComplate();
                    BelongCity.OnBuildingUpgradeComplete(this, builder);
                }
            }
            else
            {
                if (LeftCounter > 0)
                    LeftCounter--;
                ActionOver = false;
            }
            // 暂时写死
            if (isComplate && BuildingType.atk > 0 && BuildingType.atkRange > 0)
            {
                for (int i = 1; i < effectCells.Count; i++)
                {
                    Cell cell = effectCells[i];
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

            GameEvent.OnBuildingTurnStart?.Invoke(this, scenario);

            if (Render != null)
                Render.UpdateRender();

            return base.OnForceTurnStart(scenario);
        }

        public override bool OnForceTurnEnd(Scenario scenario)
        {
            GameEvent.OnBuildingTurnEnd?.Invoke(this, scenario);
            return base.OnForceTurnEnd(scenario);
        }

        public override void OnComplate(SangoObject builder)
        {
            Troop atk = builder as Troop;
            if (atk == null) return;

            Scenario scenario = Scenario.Cur;
            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Build;
            int meritGain = JobType.GetJobLimit(jobId);
            int techniquePointGain = JobType.GetJobTPGain(jobId);

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
            Tools.OverrideData<int> overrideData = GameUtility.IntOverrideData.Set(techniquePointGain);
            GameEvent.OnCityJobGainTechniquePoint?.Invoke(BelongCity, jobId, Workers.objects.ToArray(), overrideData);
            techniquePointGain = overrideData.Value;

            BelongForce.GainTechniquePoint(techniquePointGain);
            Render.UpdateRender();

        }

        public virtual void OnBuildComplate()
        {
            Scenario scenario = Scenario.Cur;
            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Build;
            int meritGain = JobType.GetJobLimit(jobId);
            int techniquePointGain = JobType.GetJobTPGain(jobId);

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            for (int i = 0; i < Builder.Count; i++)
            {
                Person person = Builder[i];
                person.merit += meritGain;
                person.GainExp(meritGain);
#if SANGO_DEBUG
                stringBuilder.Append(person.Name);
                stringBuilder.Append(" ");
#endif
                person.ClearMission();
                person.ActionOver = false;
            }
#if SANGO_DEBUG
            Sango.Log.Print($"[{BelongCity.Name}]{stringBuilder}完成{Name}建造!!");
#endif
            Tools.OverrideData<int> overrideData = GameUtility.IntOverrideData.Set(techniquePointGain);
            GameEvent.OnCityJobGainTechniquePoint?.Invoke(BelongCity, jobId, Builder.objects.ToArray(), overrideData);
            techniquePointGain = overrideData.Value;

            BelongForce.GainTechniquePoint(techniquePointGain);
            Builder = null;
        }

        public void OnUpgradeComplate()
        {
            Scenario scenario = Scenario.Cur;
            ScenarioVariables variables = scenario.Variables;
            int jobId = (int)CityJobType.Build;
            int meritGain = JobType.GetJobLimit(jobId);
            int techniquePointGain = JobType.GetJobTPGain(jobId);

            BuildingType nextBuildingType = scenario.GetObject<BuildingType>(BuildingType.nextId);
            BuildingType = nextBuildingType;

            durability = DurabilityLimit;

#if SANGO_DEBUG
            StringBuilder stringBuilder = new StringBuilder();
#endif
            for (int i = 0; i < Builder.Count; i++)
            {
                Person person = Builder[i];
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
            Tools.OverrideData<int> overrideData = GameUtility.IntOverrideData.Set(techniquePointGain);
            GameEvent.OnCityJobGainTechniquePoint?.Invoke(BelongCity, jobId, Builder.objects.ToArray(), overrideData);
            techniquePointGain = overrideData.Value;

            BelongForce.GainTechniquePoint(techniquePointGain);
            Builder = null;
        }

        public void ChangeCity(City dest)
        {
            if (!isComplate)
            {
                Sango.Log.Error("不允许转换一个未建好的建筑!!");
                return;
            }

            BelongCity.allBuildings.Remove(this);
            dest.allBuildings.Add(this);

            BelongCorps = dest.BelongCorps;
            BelongForce = dest.BelongForce;

            Render?.UpdateRender();
        }

        public Corps ChangeCorps(Corps corps)
        {
            Corps last = null;
            if (!isComplate)
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

        public override void Clear()
        {
            if (actionList != null)
            {
                for (int i = 0; i < actionList.Count; i++)
                    actionList[i].Clear();

                actionList.Clear();
                actionList = null;
            }

            Scenario.Cur.buildingSet.Remove(this);


            if (Builder != null)
            {
                for (int i = 0; i < Builder.Count; i++)
                {
                    Person person = Builder[i];
                    person.ClearMission();
                }
                Builder = null;
            }

            //if (Workers != null)
            //{
            //    for (int i = 0; i < Workers.Count; i++)
            //    {
            //        Person person = Workers[i];
            //        person.ClearMission();
            //    }
            //    Workers = null;
            //}

            if (effectCells != null)
                effectCells.Clear();
            if (CenterCell != null)
            {
                CenterCell.building = null;

                if (CenterCell.IsInterior)
                    CenterCell.CreateInteriorModel();

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
            Clear();
        }
    }
}
