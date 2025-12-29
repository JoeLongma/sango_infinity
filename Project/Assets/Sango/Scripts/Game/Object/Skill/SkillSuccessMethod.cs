using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game
{
    public abstract class SkillSuccessMethod
    {
        public SkillInstance master;
        public virtual void Init(JObject p, SkillInstance master) { this.master = master; }
        public abstract int Calculate(SkillInstance skillInstance, Troop troop, Cell spellCell);
        public virtual void Clear() { }

        public delegate SkillSuccessMethod SkillSuccessMethodCreator();

        public static Dictionary<string, SkillSuccessMethodCreator> CreateMap = new Dictionary<string, SkillSuccessMethodCreator>();
        public static void Register(string name, SkillSuccessMethodCreator action)
        {
            CreateMap[name] = action;
        }
        public static SkillSuccessMethod CraeteHandle<T>() where T : SkillSuccessMethod, new()
        {
            return new T();
        }
        public static SkillSuccessMethod Create(string name)
        {
            SkillSuccessMethodCreator creator;
            if (CreateMap.TryGetValue(name, out creator))
                return creator();
            return null;
        }

        public static void Init()
        {

            Register("FireSuccess", CraeteHandle<FireSuccess>);

        }


        public class FireSuccess : SkillSuccessMethod
        {
            /*
             1)	火计：
                    V1 = (A智*0.3-B智*0.2)+55+C+G
                    V2 = (A智*A智*(100-B智*0.9)*100/(A智*A智+B智*B智))*1/55-(100-A智)*0.1+F-D+C-5
                    火计成功率 = MIN(V1,V2)
                    注：对火计而言，如果被放计对象不是部队，则“B智”值为0
                    C：如果被用计部队是异常状态则为10，否则为0
                    D：如果被用计方智力高于用计方则为 = (B智-A智)/3，否则为0
                    F：不同地形的修正值，草地=20 土=20 沙地=20 湿地=5 毒泉=5 森林=25 荒地=17 主径=15 栈道=7 渡所=10 浅滩=5 小径=5 其它地形(川、河、海、岸、崖、港、关、城市)=0
                    G：如果放火对象不是部队则为10，否则为0

             
             */

            public override int Calculate(SkillInstance skillInstance, Troop troop, Cell spellCell)
            {
                int G = 10;
                int B = 0;
                int C = 0;
                int D = 0;
                Troop target = spellCell.troop;
                if (target != null)
                { 
                    G = target.Intelligence; 
                    if(target.HasControlBuff())
                        C = 10;
                    D = target.Intelligence > troop.Intelligence ? (target.Intelligence - troop.Intelligence) / 3 : 0;
                }
                int F = spellCell.TerrainType.fireRate;
                int V1 = (troop.Intelligence * 30 - B * 20) / 100 + 55 + C + G;
                int V2 = (troop.Intelligence * troop.Intelligence * (100 - target.Intelligence * 90 / 100) * 100 /
                    (troop.Intelligence * troop.Intelligence + target.Intelligence * target.Intelligence)) / 55 - (100 - troop.Intelligence) * 10 / 100 + F - D + C - 5;
                
                return Mathf.Min(V1, V2);
            }
        }



    }
}
