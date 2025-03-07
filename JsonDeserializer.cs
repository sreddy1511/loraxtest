using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace JsonTests
{
    public interface IJsonDeserializer
    {
        List<T> DeserializeList<T>(string filePath);
    }

    public class JsonDeserializer : IJsonDeserializer
    {
        public List<T> DeserializeList<T>(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            try
            {
                string jsonContent = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(jsonContent)
                    ?? throw new JsonException("Deserialized object is null");
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Failed to deserialize JSON from {filePath}: {ex.Message}", ex);
            }
            catch (IOException ex)
            {
                throw new IOException($"Error reading file {filePath}: {ex.Message}", ex);
            }
        }
    }
}
