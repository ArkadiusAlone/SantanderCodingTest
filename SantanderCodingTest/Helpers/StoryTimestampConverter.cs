using Newtonsoft.Json;

namespace SantanderCodingTest.Helpers
{
    public class StoryTimestampConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Integer)
            {
                var timestamp = (long)reader.Value;
                return DateTimeOffset.FromUnixTimeSeconds(timestamp);
            }

            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            throw new NotImplementedException("WriteJson is not implemented for UnixTimestampConverter");
        }
    }
}
