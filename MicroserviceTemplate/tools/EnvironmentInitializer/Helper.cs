using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace EnvironmentInitializer
{
    public static class Helper
    {
        public static void UpdateTargetPropertyJson(string targetJson, string newKmsKeyId, string newKmsPublicKey)
        {
            var targetConfiguration = File.ReadAllText(targetJson);
            JsonNode? jsonNode = JsonSerializer.Deserialize<JsonNode?>(targetConfiguration);
            jsonNode!["ModuleConfiguration"]!["Infrastructure"]!["Kms"]!["SigningKeyId"] = newKmsKeyId;
            jsonNode!["ModuleConfiguration"]!["Infrastructure"]!["Kms"]!["PublicKey"] = newKmsPublicKey;
            File.WriteAllText(targetJson, JsonSerializer.Serialize(jsonNode, new JsonSerializerOptions() { WriteIndented = true }));
        }

        public static void UpdateTargetPropertyXml(string targetXml, string newKmsKeyId, string newKmsPublicKey)
        {
            var xmlDocument = new XmlDocument
            {
                PreserveWhitespace = true //to preserve formatting this must be done before loading so that the whitespace doesn't get thrown away at load time
            };

            xmlDocument.Load(targetXml);

            XmlNode? node = xmlDocument.SelectSingleNode("//add[@key='amazonKmsSigningKeyId']");
            if (node != null && node.Attributes != null)
            {
                node.Attributes["value"]!.Value = newKmsKeyId;
            }

            node = xmlDocument.SelectSingleNode("//add[@key='amazonKmsPublicKey']");
            if (node != null && node.Attributes != null)
            {
                node.Attributes["value"]!.Value = newKmsPublicKey;
            }

            xmlDocument.Save(targetXml);
        }

        public static string? RunCommand(ConsoleMode consoleMode, string command, string? directory = null, bool waitForExit = true)
        {
            var fileName = string.Empty;
            var arguments = string.Empty;

            try
            {
                switch (consoleMode)
                {
                    case ConsoleMode.CommandPrompt:
                        fileName = "cmd.exe";
                        arguments = $"/C {command}";
                        break;
                    case ConsoleMode.Powershell:
                        fileName = "powershell.exe";
                        arguments = $" -c {command}";
                        break;
                    default:
                        break;
                }


                var process = new Process();
                if (!string.IsNullOrEmpty(directory))
                {
                    process.StartInfo.WorkingDirectory = directory;
                }
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                if (waitForExit)
                {
                    var output = string.Empty;
                    while (!process.StandardOutput.EndOfStream)
                    {
                        output = process.StandardOutput.ReadLine();
                        Console.WriteLine(output);
                    }

                    return output;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetJsonPropertyValue(string jsonKey, string output)
        {
            try
            {
                //JsonDocumentPath package is required because SelectElement is not natively supported by .NET 6 (https://stackoverflow.com/questions/70678718/how-to-delete-and-update-based-on-a-path-in-system-text-json-net-6)
                var jsonDocument = JsonDocument.Parse(output);
                JsonElement? jsonElement = jsonDocument.RootElement.SelectElement($"$.{jsonKey}");
                if (jsonElement == null)
                {
                    return string.Empty;
                }
                return jsonElement.Value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void KillProcess(string name)
        {
            Process[] workers = Process.GetProcessesByName(name);
            foreach (var worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
        }
    }

    public enum ConsoleMode
    {
        CommandPrompt,
        Powershell
    }
}