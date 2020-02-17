using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ofl.Twitch
{
    internal class DurationJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Validate parameters.
            if (typeToConvert == null) throw new ArgumentNullException(nameof(typeToConvert));
            if (options == null) throw new ArgumentNullException(nameof(options));

            // If this is across multiple spans, throw.
            if (reader.HasValueSequence)
                throw new JsonException($"The {nameof(DurationJsonConverter)} does not support reading from a value sequence.");

            // Read the span and return.
            TimeSpan? value = reader.ValueSpan.ParseDuration();

            // If null, throw.
            if (value == null) throw new JsonException($"Could not parse the duration \"{reader.GetString()}\".");

            // Return the value.
            return value.Value;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options) =>
            throw new NotImplementedException();
    }
}
