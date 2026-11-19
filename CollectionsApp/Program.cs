using System.Globalization;
using System.Text;

namespace CollectionsApp;

internal class Program
{
    private static void Main()
    {
        // дробові числа виводяться з крапкою незалежно від регіональних налаштувань
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        Console.OutputEncoding = Encoding.UTF8;
        // UTF-16 input makes .NET read the console with ReadConsoleW, so і, ї, є, ґ are not lost
        if (!Console.IsInputRedirected)
            Console.InputEncoding = Encoding.Unicode;
        Console.Title = "Класи-колекції (варіант 2)";

        var queueDemo = new QueueDemo();
        var flightsDemo = new FlightsDictionaryDemo();

        while (true)
        {
            if (!Console.IsOutputRedirected)
                Console.Clear();
            Console.WriteLine("Класи-колекції в мові C# (варіант 2)");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine(" 1 - Завдання 1: клас Queue для значень різних типів (string, double, int)");
            Console.WriteLine(" 2 - Завдання 2: Dictionary<string, Airplane> для рейсів з л.р. №7");
            Console.WriteLine(" 0 - Вихід");
            Console.WriteLine(new string('-', 70));

            switch (ConsoleInput.ReadInt("Ваш вибір: ", 0, 2))
            {
                case 0: return;
                case 1: queueDemo.Run(); break;
                case 2: flightsDemo.Run(); break;
            }
        }
    }
}
