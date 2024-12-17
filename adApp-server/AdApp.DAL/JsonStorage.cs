﻿using System.Text.Json;

namespace AdApp.DAL
{
    public class JsonStorage
    {
        public T? Load<T>(string filePath)
        {
            if (!File.Exists(filePath)) return default;
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }

        public void Save<T>(string filePath, T data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }

}
