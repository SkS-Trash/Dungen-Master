using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Progress;
using UnityEngine;

namespace Services.SaveLoadData
{
    /// <summary>
    /// Сервис для сохранения и загрузки данных локально (с учетом DynamicSaveData).
    /// </summary>
    public class SaveLoadLocalDataService : ISaveLoadDataService
    {
        private const string FOLDER_NAME = "SaveLoadData";

        private JsonSerializerSettings _jsonSettings;

        public SaveLoadLocalDataService()
        {
            InitDynamicTypes();
        }

        public void Save<T>(T data, string key)
        {
            var path = GetPath(key);
            var tmp = path + ".tmp";

            try
            {
                var json = JsonConvert.SerializeObject(data, _jsonSettings);
                File.WriteAllText(tmp, json);

                if (File.Exists(path))
                {
                    File.Replace(tmp, path, null);
                }
                else
                {
                    File.Move(tmp, path);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveLoad] Не удалось сохранить «{key}» в {path}: {ex}");
            }
        }

        public T Load<T>(string key)
        {
            var path = GetPath(key);
            if (!File.Exists(path)) return default;

            try
            {
                var json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveLoad] Не удалось загрузить «{key}» из {path}: {ex}");
                return default;
            }
        }

        private static string GetPath(string key)
        {
            var dir = Path.Combine(Application.persistentDataPath, FOLDER_NAME);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return Path.Combine(dir, key + ".json");
        }

        private void InitDynamicTypes()
        {
            var dynamicTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a =>
                {
                    Type[] types;
                    try
                    {
                        types = a.GetTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        types = e.Types;
                    }

                    return types;
                })
                .Where(t => t is not null
                            && typeof(DynamicSaveData).IsAssignableFrom(t)
                            && !t.IsAbstract)
                .ToList();

            var binder = new KnownTypesBinder
            {
                KnownTypes = dynamicTypes
            };

            _jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = binder,
                Formatting = Formatting.Indented
            };
        }
    }
}