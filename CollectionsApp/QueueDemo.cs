using System.Collections;
using System.Globalization;

namespace CollectionsApp;

/// <summary>Завдання 1: можливості класу Queue (черга, FIFO) для значень типів string, double та int.</summary>
internal class QueueDemo
{
    // необобщена черга зберігає елементи як object, тому в одній черзі можуть бути значення різних типів
    private readonly Queue queue = new();

    public void Run()
    {
        if (!Console.IsOutputRedirected)
            Console.Clear();
        Console.WriteLine("Завдання 1. Клас System.Collections.Queue - черга (першим прийшов - першим вийшов)");
        Console.WriteLine("Об'єкт колекції: Queue queue = new Queue();");
        Console.WriteLine(new string('-', 70));
        Console.WriteLine(" 1 - Додати значення в кінець черги (Enqueue)");
        Console.WriteLine(" 2 - Вилучити значення з початку черги (Dequeue)");
        Console.WriteLine(" 3 - Переглянути перше значення без вилучення (Peek)");
        Console.WriteLine(" 4 - Пошук значення в черзі (Contains)");
        Console.WriteLine(" 5 - Кількість елементів та вміст черги (Count)");
        Console.WriteLine(" 6 - Очистити чергу (Clear)");
        Console.WriteLine(" 7 - Узагальнені черги Queue<string>, Queue<double>, Queue<int>");
        Console.WriteLine(" 0 - Повернутися до головного меню");
        Console.WriteLine(new string('-', 70));

        while (true)
        {
            Console.WriteLine();
            switch (ConsoleInput.ReadInt("Операція (0-7): ", 0, 7))
            {
                case 0: return;
                case 1: Enqueue(); break;
                case 2: Dequeue(); break;
                case 3: Peek(); break;
                case 4: Find(); break;
                case 5: ShowCount(); break;
                case 6: Clear(); break;
                case 7: GenericQueues(); break;
            }
        }
    }

    private void Enqueue()
    {
        object value = ReadValue("Значення (рядок, дійсне або ціле число): ");
        queue.Enqueue(value);
        ConsoleInput.WriteColored($"  Enqueue({Describe(value)}) - значення додано в кінець черги, Count = {queue.Count}",
                                  ConsoleColor.Green);
    }

    private void Dequeue()
    {
        try
        {
            object? value = queue.Dequeue();
            Console.WriteLine($"  Dequeue() = {Describe(value)} - значення вилучено з початку черги, Count = {queue.Count}");
        }
        catch (InvalidOperationException)
        {
            WriteEmptyQueueError("Dequeue()");
        }
    }

    private void Peek()
    {
        try
        {
            object? value = queue.Peek();
            Console.WriteLine($"  Peek() = {Describe(value)} - перше значення залишається в черзі, Count = {queue.Count}");
        }
        catch (InvalidOperationException)
        {
            WriteEmptyQueueError("Peek()");
        }
    }

    private void Find()
    {
        object value = ReadValue("Шукане значення: ");
        if (!queue.Contains(value))
        {
            Console.WriteLine($"  Contains({Describe(value)}) = False - такого значення в черзі немає");
            return;
        }

        int position = 1;
        foreach (object? item in queue)
        {
            if (Equals(item, value))
                break;
            position++;
        }
        Console.WriteLine($"  Contains({Describe(value)}) = True - позиція від початку черги: {position} з {queue.Count}");
    }

    private void ShowCount()
    {
        Console.WriteLine($"  Count = {queue.Count}");
        if (queue.Count == 0)
        {
            Console.WriteLine("  Черга порожня");
            return;
        }

        Console.WriteLine("  Вміст від початку до кінця: " + string.Join(", ", queue.ToArray().Select(Describe)));

        // елементи мають тип object, тому для обчислень потрібна перевірка типу і розпакування
        int strings = 0, doubles = 0, ints = 0;
        double sum = 0;
        foreach (object? item in queue)
        {
            switch (item)
            {
                case string: strings++; break;
                case double d: doubles++; sum += d; break;
                case int i: ints++; sum += i; break;
            }
        }
        Console.WriteLine($"  За типами: string - {strings}, double - {doubles}, int - {ints}; сума чисел = {sum:0.###}");
    }

    private void Clear()
    {
        queue.Clear();
        ConsoleInput.WriteColored($"  Clear() - чергу очищено, Count = {queue.Count}", ConsoleColor.Green);
    }

    private static void GenericQueues()
    {
        Console.WriteLine("  Узагальнені черги зберігають значення одного типу без упаковки та приведення типів:");

        var cities = new Queue<string>(["Тернопіль", "Київ"]);
        cities.Enqueue("Львів");
        var prices = new Queue<double>();
        prices.Enqueue(1250.5);
        prices.Enqueue(3999.99);
        prices.Enqueue(780.25);
        var seats = new Queue<int>([180, 220]);
        seats.Enqueue(150);

        Print("Queue<string> cities", cities);
        Print("Queue<double> prices", prices);
        Print("Queue<int>    seats ", seats);

        string first = cities.Dequeue();
        Console.WriteLine($"  cities.Dequeue() = {first}; cities.Peek() = {cities.Peek()}; " +
                          $"cities.Contains(\"Львів\") = {cities.Contains("Львів")}; cities.Count = {cities.Count}");
        Console.WriteLine($"  prices.Sum() = {prices.Sum():0.##}; prices.Max() = {prices.Max():0.##} - обчислення без приведення типів");

        bool taken = seats.TryDequeue(out int seat);
        Console.WriteLine($"  seats.TryDequeue(out seat) = {taken}, seat = {seat}; seats.Count = {seats.Count}");
        seats.Clear();
        taken = seats.TryDequeue(out seat);
        Console.WriteLine($"  seats.Clear(); seats.TryDequeue(out seat) = {taken} - черга порожня, виняток не виникає");
        Console.WriteLine("  cities.Enqueue(42) не скомпілюється: у Queue<string> можна додавати лише рядки");
    }

    private static void Print<T>(string title, Queue<T> items) =>
        Console.WriteLine($"  {title}: {string.Join(", ", items)} (Count = {items.Count})");

    /// <summary>Визначає тип введеного значення: ціле число, дійсне число або рядок.</summary>
    private static object ReadValue(string prompt)
    {
        string text = ConsoleInput.ReadText(prompt);
        if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
            return intValue;
        if (double.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleValue)
            && double.IsFinite(doubleValue))
            return doubleValue;
        return text;
    }

    private static string Describe(object? value) => value switch
    {
        string s => $"\"{s}\" (string)",
        double d => $"{d.ToString(CultureInfo.InvariantCulture)} (double)",
        int i => $"{i} (int)",
        null => "null",
        _ => $"{value} ({value.GetType().Name})"
    };

    private static void WriteEmptyQueueError(string operation) =>
        ConsoleInput.WriteColored($"  Виняток InvalidOperationException: {operation} неможливо виконати, черга порожня",
                                  ConsoleColor.Red);
}
