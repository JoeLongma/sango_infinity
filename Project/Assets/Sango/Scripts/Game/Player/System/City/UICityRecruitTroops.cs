namespace Sango.Game.Render.UI
{
    public class UICityRecruitTroops : UICityComandBase
    {
        public override void OnInit()
        {
            base.OnInit();
            title1.gameObject.SetActive(false);
            value_1.gameObject.SetActive(false);
            title_value.text = "士兵";
            title_gold.text = "资金";
            value_cur.text = currentSystem.TargetCity.troops.ToString();
            value_dest.text = (currentSystem.TargetCity.troops + currentSystem.wonderNumber).ToString();
            value_gold.text = $"{currentSystem.TargetCity.CheckJobCost(CityJobType.RecuritTroops)}/{currentSystem.TargetCity.gold}";
        }
    }
}
