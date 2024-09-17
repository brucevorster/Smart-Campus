using System.Text.Json;

namespace UmojaCampus.Client.Helpers
{
    public static class Serializations
    {
        public static string SerializeObject<T>(T obj)
            => JsonSerializer.Serialize(obj);

        public static T DeserializeJsonString<T>(string jsonString)
            => JsonSerializer.Deserialize<T>(jsonString);

        public static IList<T> DeserializeJsonStringList<T>(string jsonString)
            => JsonSerializer.Deserialize<IList<T>>(jsonString);
    }
}
