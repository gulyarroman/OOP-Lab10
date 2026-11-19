using System.Globalization;
using SimpleClassLibrary;

namespace CollectionsApp;

/// <summary>Введення даних з клавіатури з перевіркою коректності.</summary>
internal static class ConsoleInput
{
    public const string DateHint = "дд.мм.рррр гг:хх";

    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                return value;

            WriteError($"потрібне ціле число від {min} до {max}");
        }
    }

    public static string ReadText(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string text = (Console.ReadLine() ?? string.Empty).Trim();
            if (text.Length > 0 && text.Length <= 30)
                return text;

            WriteError("потрібно від 1 до 30 символів");
        }
    }

    public static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string text = (Console.ReadLine() ?? string.Empty).Trim().Replace(',', '.');
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                return value;

            WriteError("потрібне число");
        }
    }

    public static Date ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string text = (Console.ReadLine() ?? string.Empty).Trim();
            if (DateTime.TryParseExact(text, ["dd.MM.yyyy HH:mm", "d.M.yyyy H:mm"], CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out DateTime value))
                return new Date(value);

            WriteError($"потрібна дата у форматі {DateHint}");
        }
    }

    public static void WriteError(string message) =>
        WriteColored($"     Помилка введення: {message}. Повторіть введення.", ConsoleColor.Yellow);

    public static void WriteColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
