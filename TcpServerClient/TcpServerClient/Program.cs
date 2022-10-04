//https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/tcp/tcp-services

using System.Net.Sockets;
using System.Net;
using System.Text;
using InputSimulatorStandard;
using InputSimulatorStandard.Native;

var ipEndPoint = new IPEndPoint(IPAddress.Any, 8888);
TcpListener listener = new(ipEndPoint);

try
{

    Console.WriteLine($"Server starts");
    var inputSimulator = new InputSimulator();

    listener.Start();

    while (true)
    {
        using TcpClient handler = await listener.AcceptTcpClientAsync();
        await using NetworkStream stream = handler.GetStream();

        var buffer = new byte[1_024];
        int received = await stream.ReadAsync(buffer);
        var message = Encoding.UTF8.GetString(buffer, 0, received);

        /*
            https://www.twoplayergames.org/game/minibattles-2-6-players
            GAME CONTROLS:

            Player 1 : C
            Player 2 : N
            Player 3 : Q
            Player 4 : P
            Player 5 : Left Arrow
            Player 6 : Right Arrow
         */

        switch (message)
        {
            case "C":
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_C);
                break;
            case "N":
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_N);
                break;
            case "Q":
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_Q);
                break;
            case "P":
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_P);
                break;
            case "LeftArrow":
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LEFT);
                break;
            case "RightArrow":
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RIGHT);
                break;
            default:
                break;
        }
        
        Console.WriteLine($"\"{message}\" received at {DateTime.Now.ToString("O")}");
    }
}
finally
{
    listener.Stop();
}