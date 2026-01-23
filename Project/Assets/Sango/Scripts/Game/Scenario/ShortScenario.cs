using Newtonsoft.Json;
using Sango.Hexagon;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]

    public class ShortForce : SangoObject
    {
        [JsonProperty] public int Governor;
        [JsonProperty] public int Counsellor;
        [JsonProperty] public int Flag;
        public bool IsPlayer;
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class ShortPerson : SangoObject
    {
        public int BelongForce;
        public int BelongCity;
        public int headIconID;
        public int imageID;
    }


    [JsonObject(MemberSerialization.OptOut)]
    public class ShortCity : SangoObject
    {
        public int BelongForce;
        public int BuildingType;
        public int x;
        public int y;
        public int troops;
        public int gold;
        public int food;
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class ShortMap
    {
        public int Width;
        public int Height;
        public string Name;
        public float GridSize;
        HexWorld HexWorld;
        public void Init(ShortScenario scenario)
        {
            string mapName = scenario.Info.mapType;
            string FileName = Path.FindFile($"Map/{mapName}.bin");
            if (File.Exists(FileName))
            {
                Name = mapName;
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(fs);
                int versionCode = reader.ReadInt32();
                if (versionCode < 6)
                {
                    return;
                }
                reader.ReadString();
                int mapWidth = reader.ReadInt32();
                int mapHeight = reader.ReadInt32();
                int grid_size = reader.ReadInt32();
                Width = mapWidth / 4;
                Height = mapHeight / 4;
                GridSize = grid_size;

                reader.Close();
                fs.Close();
                reader.Dispose();
                fs.Dispose();
            }

            HexWorld = new Hexagon.HexWorld(new Hexagon.Point(GridSize, GridSize), new Hexagon.Point(0, 0));

        }
        public Vector3 Coords2Position(int x, int y)
        {
            return HexWorld.CoordsToPosition(x, y);
        }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ShortScenario
    {
        [JsonProperty(Order = -99)]
        private int _Id = -1;
        public int Id { get { return _Id; } set { _Id = value; } }

        [JsonProperty(Order = -98)]
        public virtual string Name { get; set; }

        #region Data
        [JsonProperty] public ScenarioInfo Info { get; internal set; }
        [JsonProperty] public ScenarioCommonData CommonData { internal set; get; }
        [JsonProperty] public ScenarioVariables Variables { internal set; get; }
        [JsonProperty] public ShortMap Map { internal set; get; }
        [JsonProperty] public Dictionary<int, ShortForce> forceSet = new Dictionary<int, ShortForce>();
        [JsonProperty] public Dictionary<int, ShortPerson> personSet = new Dictionary<int, ShortPerson>();
        [JsonProperty] public Dictionary<int, ShortCity> citySet = new Dictionary<int, ShortCity>();
        #endregion Data
        public static ShortScenario Cur { get; private set; }
        public static List<ShortScenario> all_scenario_info_list = new List<ShortScenario>();
        public static ShortScenario CurSelected { get; set; }

        public string FilePath { internal set; get; }

        public ShortScenario(string filePath)
        {
            this.FilePath = filePath;
            LoadContent();
        }
        public static void Add(string path)
        {
            if (!File.Exists(path))
                return;

            ShortScenario scenario = new ShortScenario(path);

            all_scenario_info_list.Add(scenario);
        }

        public void LoadContent()
        {
            LoadContent(FilePath);
        }

        public void LoadContent(string path)
        {
            Cur = this;
            if (CommonData == null)
                CommonData = GameData.Instance.LoadNewCommonData();
            if (Variables == null)
                Variables = new ScenarioVariables();
            if (Map == null)
                Map = new ShortMap();
            JsonConvert.PopulateObject(File.ReadAllText(FilePath), this);

            // 玩家确定
            if (Info.playerForceList != null && Info.playerForceList.Length > 0)
            {
                foreach (var x in forceSet.Values)
                {
                    for (int k = 0; k < Info.playerForceList.Length; k++)
                    {
                        if (Info.playerForceList[k] == x.Id)
                        {
                            x.IsPlayer = true;
                            break;
                        }
                    }
                }
            }
            Map.Init(this);
        }
    }
}
