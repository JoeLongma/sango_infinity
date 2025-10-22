using Newtonsoft.Json;
using System;

namespace Sango.Game
{
    /// <summary>
    /// 这个比较特殊,包括继承类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SangoObjectSetCityConverter : JsonConverter<SangoObjectSet<City>>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            SangoObjectSet<City> dest = value as SangoObjectSet<City>;
            writer.WritePropertyName("0");
            serializer.Serialize(writer, dest.Default);
            dest.ForEach(x =>
            {
                writer.WritePropertyName(x.Id.ToString());
                serializer.Serialize(writer, x);
            });
            writer.WriteEndObject();
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (existingValue == null)
                existingValue = Create(objectType);
            SangoObjectSet<City> dest = existingValue as SangoObjectSet<City>;
            string lastPropertyName = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    lastPropertyName = reader.Value.ToString();
                }
                else if (reader.TokenType == JsonToken.StartObject)
                {
                    if (!string.IsNullOrEmpty(lastPropertyName))
                    {
                        int Id = int.Parse(lastPropertyName);
                        City exsist = dest.Get(Id);
                        if (exsist != null)
                        {
                            serializer.Populate(reader, exsist);
                            continue;
                        }

                        if (Id >= Scenario.Cur.Variables.PortBeginIndex)
                        {
                            Port v = serializer.Deserialize<Port>(reader);
                            dest.Add(v);
                        }
                        else if (Id >= Scenario.Cur.Variables.GateBeginIndex)
                        {
                            Gate v = serializer.Deserialize<Gate>(reader);
                            dest.Add(v);
                        }
                        else
                        {
                            City v = serializer.Deserialize<City>(reader);
                            dest.Add(v);
                        }
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    return dest;
                }
            }
            return dest;
        }
    }
}
