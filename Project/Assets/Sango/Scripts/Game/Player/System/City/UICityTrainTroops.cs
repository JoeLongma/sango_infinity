namespace Sango.Game.Render.UI
{
    public class UICityTrainTroops : UICityComandBase
    {
        public override void OnInit()
        {
            base.OnInit();
            title1.gameObject.SetActive(false);
            value_1.gameObject.SetActive(false);
            title_gold.gameObject.SetActive(false);
            value_gold.gameObject.SetActive(false);
            title_value.text = "士气";
            value_cur.text = currentSystem.TargetCity.morale.ToString();
            value_dest.text = (currentSystem.TargetCity.morale + currentSystem.wonderNumber).ToString();
        }
    }
}
