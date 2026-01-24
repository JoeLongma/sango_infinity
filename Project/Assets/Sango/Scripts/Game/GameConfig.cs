namespace Sango.Game
{
    public static class GameConfig
    {
        public static int WindowWidth;
        public static int WindowHeight;
        public static int Language;
        public static string SavePath;
        
        public static void Init()
        {
            SavePath = Path.ContentRootPath + "/Save";
        }
    }
}
