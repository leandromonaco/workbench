using System.Net.Sockets;
using System.Net;
using System.Text;

var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);

var key = Console.ReadKey();
while (key.Key != ConsoleKey.Escape)
{
    using TcpClient client = new();
    await client.ConnectAsync(ipEndPoint);
    await using NetworkStream stream = client.GetStream();

    var message = $"{key.Key}";
    var dateTimeBytes = Encoding.UTF8.GetBytes(message);
    await stream.WriteAsync(dateTimeBytes);
    Console.WriteLine($"\"{message}\" sent at {DateTime.Now.ToString("O")}");
    Console.WriteLine();
    key = Console.ReadKey();
}
