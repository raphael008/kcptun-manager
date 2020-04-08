using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace KcptunManager
{
    public static class ConfigHelper
    {
        private static readonly string configPath = "config.json";

        public static List<Config> ReadConfig()
        {
            if (File.Exists(configPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.DefaultValueHandling = DefaultValueHandling.Populate;

                using (StreamReader sr = new StreamReader(configPath))
                {
                    using (JsonReader jr = new JsonTextReader(sr))
                    {
                        return serializer.Deserialize<List<Config>>(jr) ?? new List<Config>();
                    }
                }
            }

            return new List<Config>();
        }

        public static void WriteConfig(List<Config> configs)
        {
            if (!File.Exists(configPath))
            {
                File.Create(configPath).Close();
            }

            if (configs.Count > 1)
                configs.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));


            JsonSerializer serializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Populate
            };

            using (StreamWriter sw = new StreamWriter(configPath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, configs);
                }
            }
        }
    }
}