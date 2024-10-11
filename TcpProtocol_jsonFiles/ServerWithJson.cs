using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using TcpProtocol_jsonFiles.Common;

namespace TcpProtocol_jsonFiles_Server;

public class ServerWithJson
{
    private TcpListener _listener;
    private IPEndPoint _ipEndPoint = new IPEndPoint(IPAddress.Any, 12000);

    public void StartServer()
    {
        _listener = new TcpListener(_ipEndPoint);
        _listener.Start();
        Console.WriteLine("Server is started!");

        while (true)
        {
            Task.Run(() =>
            {
                using var client = _listener.AcceptTcpClient();
                var clientConnected = true;

                using var streamReader = new StreamReader(client.GetStream());
                using var streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };

                while (clientConnected)
                {
                    var json = streamReader.ReadLine();
                    var command = JsonSerializer.Deserialize<Command>(json);
                    var result = "";

                    switch (command?.Operation)
                    {
                        case OperationEnum.Random:
                            result = $"The result is: {Operations.RandomNumbers(command.Numbers)}";
                            break;

                        case OperationEnum.Add:
                            result = $"The result is: {Operations.AddNumbers(command.Numbers)}";
                            break;

                        case OperationEnum.Subtract:
                            result = $"The result is: {Operations.SubtractNumbers(command.Numbers)}";
                            break;

                        case OperationEnum.CloseConnection:
                            clientConnected = false;
                            break;

                        default:
                            streamWriter.WriteLine("No valid command chosen");
                            break;
                    }
                    streamWriter.WriteLine(result);
                }
            });
        }

    }

}
