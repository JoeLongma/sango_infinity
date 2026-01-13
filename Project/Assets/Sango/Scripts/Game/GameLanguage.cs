using Newtonsoft.Json;
using Sango.Mod;
using System.Collections.Generic;
using System.IO;

namespace Sango.Game
{
    public class GameLanguage : Singleton<GameLanguage>
    {
        string LanguageFile = "Data/Language/Language.json";
        public class StringTable
        {
            public int Id;
            public string value;
        }
        public Dictionary<int, StringTable> curLanguage = new Dictionary<int, StringTable>();

        public void Init(string lanName)
        {
            ModManager.Instance.LoadFile(LanguageFile, filewhere =>
            {
                using (StreamReader file = System.IO.File.OpenText(filewhere))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    while (reader.Read()) // Advances to the next token in the JSON stream.
                    {
                        if (reader.TokenType == JsonToken.StartObject) // Check for start of an object in the JSON stream.
                        {
                            if (!string.IsNullOrEmpty(reader.Path) && reader.Path == lanName)
                            {
                                JsonSerializer.CreateDefault().Populate(reader, curLanguage); // Deserialize the object.
                                return;
                            }
                        }
                    }
                }
            });

            foreach (StringTable value in curLanguage.Values)
            {
                value.value = value.value.Replace("\\n", "\n");
            }

        }

        public void _ChangeLanguage(string language)
        {
            Init(language);
        }

        private string _GetString(int id)
        {
            if (curLanguage.TryGetValue(id, out var str))
                return str.value;
            return $"text not find: {id}";
        }

        public static string GetString(int id)
        {
            return Instance._GetString(id);
        }

        public static void ChangeLanguage(string language)
        {
            Instance._ChangeLanguage(language);
        }
    }
}
