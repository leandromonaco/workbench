using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CodeKata.CSharp.Helpers.Kata4
{
    internal class JobCreated
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
