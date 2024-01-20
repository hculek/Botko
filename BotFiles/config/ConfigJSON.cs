using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Botko.BotFiles.config
{
    public struct ConfigJSON
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
    }

    public static class ConfigProps
    {
        public static async Task<ConfigJSON> Get()
        {
            return JsonSerializer.Deserialize<ConfigJSON>(File.ReadAllText("config.json"));
        }
    
    }
}
