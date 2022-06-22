using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace CmdRunner
{
    public class Helper
    {
        public static void UpdateTargetPropertyJson(string targetJson, string newKmsKeyId, string newKmsPublicKey)
        {
            var targetConfiguration = File.ReadAllText(targetJson);
            JsonNode jsonNode = JsonSerializer.Deserialize<JsonNode>(targetConfiguration);
            jsonNode["ModuleConfiguration"]["Infrastructure"]["Kms"]["SigningKeyId"] = newKmsKeyId;
            jsonNode["ModuleConfiguration"]["Infrastructure"]["Kms"]["PublicKey"] = newKmsPublicKey;
            File.WriteAllText(targetJson, JsonSerializer.Serialize(jsonNode, new JsonSerializerOptions() { WriteIndented = true }));
        }

        public static void UpdateTargetPropertyXml(string targetXml, string newKmsKeyId, string newKmsPublicKey)
        {
            var xmlDocument = new XmlDocument
            {
                PreserveWhitespace = true //to preserve formatting this must be done before loading so that the whitespace doesn't get thrown away at load time
            };

            xmlDocument.Load(targetXml);

            XmlNode node = xmlDocument.SelectSingleNode("//add[@key='amazonKmsSigningKeyId']");
            if (node != null)
            {
                node.Attributes["value"].Value = newKmsKeyId;
            }

            node = xmlDocument.SelectSingleNode("//add[@key='amazonKmsPublicKey']");
            if (node != null)
            {
                node.Attributes["value"].Value = newKmsPublicKey;
            }

            xmlDocument.Save(targetXml);
        }

        public static string RunCommand(string command)
        {
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/C {command}";
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        public static string GetJsonPropertyValue(string jsonKey, string output)
        {
            //JsonDocumentPath package is required because SelectElement is not natively supported by .NET 6 (https://stackoverflow.com/questions/70678718/how-to-delete-and-update-based-on-a-path-in-system-text-json-net-6)
            var jsonDocument = JsonDocument.Parse(output);
            JsonElement? jsonElement = jsonDocument.RootElement.SelectElement($"$.{jsonKey}");
            return jsonElement.Value.ToString();
        }
    }
}