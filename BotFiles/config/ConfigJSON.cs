using System.Text.Json;

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
