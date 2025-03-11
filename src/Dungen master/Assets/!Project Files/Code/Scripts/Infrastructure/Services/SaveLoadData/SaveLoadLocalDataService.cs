using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// Сервис для сохранения и загрузки данных локально.
    /// </summary>
    public class SaveLoadLocalDataService : ISaveLoadDataService
    {
        /// <inheritdoc/>
        public void Save<T>(T data, string key)
        {
            var path = GetPath(key);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            File.WriteAllText(path, json);
        }

        /// <inheritdoc/>
        public T Load<T>(string key)
        {
            var path = GetPath(key);

            if (!File.Exists(path))
            {
                return default;
            }

            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        private static string GetPath(string key) =>
            Application.persistentDataPath + "/" + key + ".json";
    }
}