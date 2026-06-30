using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Services.SaveLoadData
{
    /// <summary>
    /// Биндер, фиксирующий «дозволенный» список типов для сериализации / десериализации.
    /// </summary>
    public class KnownTypesBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes { get; set; }

        public Type BindToType(string assemblyName, string typeName)
        {
            // ищем по полному имени типа
            foreach (var t in KnownTypes)
                if (t.FullName == typeName)
                    return t;

            throw new JsonSerializationException($"Попытка десериализовать неизвестный тип: {typeName}");
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            // не пишем assembly, только FullName
            assemblyName = null;
            typeName = serializedType.FullName;
        }
    }
}