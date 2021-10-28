using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VirtualDean.Enties
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TrayHour
    {
        T8,
        T9,
        T10,
        T12,
        T13,
        T15,
        T17,
        T19,
        T20,
        T21
    }
}
