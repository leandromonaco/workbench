using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Globalization;

namespace IntegrationConnectors.TeamCity.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var str = reader.GetString();
            str = str.Replace("+0000", string.Empty);
            var date =  DateTime.ParseExact(str, "yyyyMMddTHHmmss", provider);
            return TimeZone.CurrentTimeZone.ToLocalTime(date);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
