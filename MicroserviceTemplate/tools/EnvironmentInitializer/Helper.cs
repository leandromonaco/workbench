using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace EnvironmentInitializer
{
    public static class Helper
    {
        public static string RunCmdCommand(string command, string? directory = null, bool waitForExit = true)
        {
            try
            {
                var p = new Process();
                if (!string.IsNullOrEmpty(directory))
                {
                    p.StartInfo.WorkingDirectory = directory;
                }
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = $"/C {command}";
                p.Start();
                if (waitForExit)
                {
                    var output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    return output;
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string RunPowerShellCommand(string command, string? directory = null, bool waitForExit = true)
        {
            try
            {
                var p = new Process();
                if (!string.IsNullOrEmpty(directory))
                {
                    p.StartInfo.WorkingDirectory = directory;
                }
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "powershell.exe";
                p.StartInfo.Arguments = $" -c {command}";
                p.Start();

                if (waitForExit)
                {
                    var output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
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
}