using System.Net.Sockets;
using System.Net;
using TcpProtocol_jsonFiles.Common;
using System.Text.Json;

namespace TcpProtocol_Json_Client;

public class Client
{
    public IPEndPoint ipEndPoint { get; set; }

    public Client()
    {
    }

    public void StartClient()
    {
        Console.WriteLine("Client is started!");

        var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 12000);
        using var client = new TcpClient();
        client.Connect(ipEndPoint);

        Console.WriteLine($"Is connected: {client.Connected}");

        using var streamReader = new StreamReader(client.GetStream());
        using var streamWriter = new StreamWriter(client.GetStream());
        streamWriter.AutoFlush = true;

        while (client.Connected)
        {
            Console.WriteLine("Write a command: 'random', 'add' or 'subtract' - Write 'close' to stop the connection to the server");

            while (client.Connected)
            {
                var operation = Console.ReadLine();

                if (operation?.ToLower() == "close")
                {
                    CloseCommand(streamWriter);
                    client.Close();
                    return;
                }

                Console.WriteLine("Input two numbers... (Example: 1 10)");
                var numbers = GetIntArrayFromInput();

                switch (operation?.ToLower())
                {
                    case "add":
                        SendCommand(OperationEnum.Add, numbers, streamWriter);
                        break;
                    case "random":
                        SendCommand(OperationEnum.Random, numbers, streamWriter);
                        break;
                    case "subtract":
                        SendCommand(OperationEnum.Subtract, numbers, streamWriter);
                        break;
                }

                var result = streamReader.ReadLine();
                Console.WriteLine(result);
                break;
            }
        }
    }

    private void CloseCommand(StreamWriter streamWriter)
    {
        var command = new Command()
        {
            Operation = OperationEnum.CloseConnection,
        };

        var jsonString = JsonSerializer.Serialize(command);
        streamWriter.WriteLine(jsonString);

        Console.WriteLine("Connection has been closed");
    }

    private void SendCommand(OperationEnum operation, int[] numbers, StreamWriter streamWriter)
    {
        var command = new Command()
        {
            Operation = operation,
            Numbers = numbers
        };

        var jsonString = JsonSerializer.Serialize(command);

        streamWriter.WriteLine(jsonString);
    }

    private static int[] GetIntArrayFromInput()
    {
        try
        {
            var input = Console.ReadLine();
            var splitString = input?.Split(" ");
            if (splitString != null && splitString.Length == 2)
            {
                var arrayNumbers = new[] { Convert.ToInt32(splitString[0]), Convert.ToInt32(splitString[1]) };
                return arrayNumbers;
            }
            Console.WriteLine("Invalid input. Please enter two numbers!");
        }
        catch (Exception)
        {
            Console.WriteLine("Error parsing numbers. Please try again!");
        }
        return null;
    }
}
