namespace TcpProtocol_jsonFiles.Common;

public class Command
{
    public int[] Numbers { get; set; }
    public OperationEnum Operation { get; set; }

    public Command(int[] numbers, OperationEnum operation)
    {
        Numbers = numbers;
        Operation = operation;
    }

    public Command()
    {
    }

    public override string ToString()
    {
        return $"Operation: '{Operation}', Number 1: '{Numbers[0]}' and Number 2: '{Numbers[1]}'";
    }
}
