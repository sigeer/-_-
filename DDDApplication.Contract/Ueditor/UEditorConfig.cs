using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DDDApplication.Contract.Ueditor
{
    public class UEditorConfig
    {
        public static void Register(string configPath)
        {
            var config = File.ReadAllText(configPath);
            var allConfig = JsonConvert.DeserializeObject<JObject>(config);
            Items = allConfig["UEditorConfig"];
        }

        public static object Items { get; set; } = null!;
    }
}
