using System.Net.NetworkInformation;

for (int i = 0; i < 200; i++)
{
    PingServer("google.com");
    Thread.Sleep(1000);
}

void PingServer(string hostName)
{
    try
    {
        var pinger = new Ping();
        pinger.Send(hostName);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} PING: {hostName} OK");
    }
    catch (Exception)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")} PING: {hostName} Failed");
    }
}