namespace TcpProtocol_jsonFiles_Server;

public static class Operations
{
    public static string AddNumbers(int[] numbers)
    {
        var numbersAdded = numbers[0] + numbers[1];
        var result = numbersAdded.ToString();
        return result;
    }

    public static string SubtractNumbers(int[] numbers)
    {
        var numbersSubtracted = numbers[0] - numbers[1];
        var result = numbersSubtracted.ToString();
        return result;
    }

    public static string RandomNumbers(int[] numbers)
    {
        var min = numbers.Min();
        var max = numbers.Max();
        var randomNumber = Random.Shared.Next(min, max);
        var result = randomNumber.ToString();
        return result;
    }
}
