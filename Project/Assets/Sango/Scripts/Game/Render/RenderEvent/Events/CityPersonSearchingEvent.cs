using Sango.Game.Render.UI;
using UnityEngine;

namespace Sango.Game.Render
{
    public class CityPersonSearchingEvent : RenderEventBase
    {
        public City city;
        public Person person;
        public Person target;
        public override void Enter(Scenario scenario)
        {
            if (!city.DoJobSearching(person, out target))
            {
                IsDone = true; 
                return;
            }

            if(!city.BelongCorps.IsPlayer)
            {
                person.JobRecuritPerson(target, 3);
                IsDone = true;
                return;
            }

            string content = $"搜索结果，\n发现了名为<color=#00ffff>{target.Name}</color>的武将。";
            UIDialog dialog = UIDialog.Open(UIDialog.DialogStyle.ClickPersonSay, content, () => {
                // TODO:展示武将
                // 暂时直接招募
                UIDialog.Close();
                if (person.JobRecuritPerson(target, 3) )
                {
                    UIDialog dialog1 = UIDialog.Open(UIDialog.DialogStyle.ClickPersonSay, $"成功招募了<color=#00ffff>{target}</color>", () => {
                        // TODO:展示武将
                        // 暂时直接招募
                        UIDialog.Close();
                        UIDialog dialog2 = UIDialog.Open(UIDialog.DialogStyle.ClickPersonSay, $"<color=#00ffff>{target}</color>愿为主公献犬马之劳", () => {
                            // TODO:展示武将
                            // 暂时直接招募
                            UIDialog.Close();
                            IsDone = true;
                        });
                        dialog2.SetPerson(target);
                    });
                    dialog1.SetPerson(person);
                }
                else
                {
                    UIDialog dialog1 = UIDialog.Open(UIDialog.DialogStyle.ClickPersonSay, $"很遗憾，\n未能招募到<color=#00ffff>{target}</color>", () => {
                        // TODO:展示武将
                        // 暂时直接招募
                        UIDialog.Close();
                        IsDone = true;
                    });
                    dialog1.SetPerson(person);
                }
            });
            dialog.SetPerson(person);
        }

        public override void Exit(Scenario scenario)
        {
            
        }

        public override bool IsVisible()
        {
            return city.BelongCorps.IsPlayer;
        }

        public override bool Update(Scenario scenario, float deltaTime)
        {
            return IsDone;
        }
    }
}
