using SimpleClassLibrary;

namespace CollectionsApp;

/// <summary>Завдання 2: можливості класу Dictionary&lt;TKey, TValue&gt; для колекції рейсів (клас Airplane з л.р. №7).</summary>
internal class FlightsDictionaryDemo
{
    // ключ - номер рейсу (регістр літер не враховується), значення - об'єкт класу Airplane
    private readonly Dictionary<string, Airplane> flights = new(StringComparer.OrdinalIgnoreCase)
    {
        ["PS-101"] = new Airplane("Тернопіль", "Варшава", new Date(2026, 11, 22, 8, 30),
                                  new Date(2026, 11, 22, 9, 55), 470),
        ["PS-231"] = new Airplane("Київ", "Нью-Йорк", new Date(2026, 11, 25, 22, 10),
                                  new Date(2026, 11, 26, 7, 40), 7483.45),
        ["VY-777"] = new Airplane("Львів", "Барселона", new Date(2026, 11, 28, 14, 0),
                                  new Date(2026, 11, 28, 17, 35), 2025)
    };

    public void Run()
    {
        if (!Console.IsOutputRedirected)
            Console.Clear();
        Console.WriteLine("Завдання 2. Клас Dictionary<string, Airplane> - словник рейсів (ключ - номер рейсу)");
        Console.WriteLine("Об'єкт колекції: new Dictionary<string, Airplane>(StringComparer.OrdinalIgnoreCase)");
        Console.WriteLine(new string('-', 70));
        Console.WriteLine(" 1 - Показати всі рейси (перебір пар KeyValuePair у циклі foreach)");
        Console.WriteLine(" 2 - Додати рейс (ContainsKey, Add)");
        Console.WriteLine(" 3 - Знайти рейс за номером (TryGetValue)");
        Console.WriteLine(" 4 - Знайти рейси за містом відправлення або прибуття (перебір Values)");
        Console.WriteLine(" 5 - Вилучити рейс за номером (Remove)");
        Console.WriteLine(" 6 - Кількість рейсів (Count, Keys)");
        Console.WriteLine(" 7 - Очистити словник (Clear)");
        Console.WriteLine(" 0 - Повернутися до головного меню");
        Console.WriteLine(new string('-', 70));

        while (true)
        {
            Console.WriteLine();
            switch (ConsoleInput.ReadInt("Операція (0-7): ", 0, 7))
            {
                case 0: return;
                case 1: ShowAll(); break;
                case 2: Add(); break;
                case 3: FindByNumber(); break;
                case 4: FindByCity(); break;
                case 5: Remove(); break;
                case 6: ShowCount(); break;
                case 7: Clear(); break;
            }
        }
    }

    private void ShowAll()
    {
        if (flights.Count == 0)
        {
            Console.WriteLine("  Словник порожній, Count = 0");
            return;
        }
        PrintTable(flights);
    }

    private void Add()
    {
        string number;
        while (true)
        {
            number = ConsoleInput.ReadText("Номер рейсу (ключ): ").ToUpperInvariant();
            if (!flights.ContainsKey(number))
                break;
            ConsoleInput.WriteError($"рейс {number} вже є у словнику, ключ має бути унікальним");
        }

        string from = ConsoleInput.ReadText("Місто відправлення: ");
        string to = ConsoleInput.ReadText("Місто прибуття: ");
        Date departure = ConsoleInput.ReadDate($"Відправлення ({ConsoleInput.DateHint}): ");
        Date arrival = ConsoleInput.ReadDate($"Прибуття ({ConsoleInput.DateHint}): ");
        double distance = ConsoleInput.ReadDouble("Дальність польоту, км: ");

        try
        {
            var airplane = new Airplane(from, to, departure, arrival, distance);
            flights.Add(number, airplane);
            ConsoleInput.WriteColored($"  Add(\"{number}\", airplane) - рейс додано до словника, Count = {flights.Count}",
                                      ConsoleColor.Green);
        }
        catch (ArgumentException ex)
        {
            ConsoleInput.WriteColored($"  Помилка: {ex.Message}. Рейс не додано.", ConsoleColor.Red);
        }
    }

    private void FindByNumber()
    {
        string number = ConsoleInput.ReadText("Номер рейсу: ");
        if (flights.TryGetValue(number, out Airplane? airplane))
        {
            Console.WriteLine($"  TryGetValue(\"{number}\") = True, знайдено рейс:");
            Console.WriteLine($"    Маршрут:      {airplane.DepartureCity} -> {airplane.ArrivalCity}");
            Console.WriteLine($"    Відправлення: {airplane.DepartureDate}");
            Console.WriteLine($"    Прибуття:     {airplane.ArrivalDate}");
            Console.WriteLine($"    Дальність:    {FormatKm(airplane.DistanceKilometers)} ({airplane.DistanceMiles:0.##} миль)");
            Console.WriteLine($"    У дорозі:     {FormatTime(airplane.GetTotalTime())}, " +
                              $"прибуття в день вильоту: {(airplane.IsArrivingToday() ? "так" : "ні")}");
        }
        else
        {
            ConsoleInput.WriteColored($"  TryGetValue(\"{number}\") = False - рейсу з таким номером немає",
                                      ConsoleColor.Yellow);
        }
    }

    private void FindByCity()
    {
        string city = ConsoleInput.ReadText("Місто: ");
        var found = flights
            .Where(pair => pair.Value.DepartureCity.Equals(city, StringComparison.OrdinalIgnoreCase)
                           || pair.Value.ArrivalCity.Equals(city, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (found.Count == 0)
        {
            ConsoleInput.WriteColored($"  Рейсів з/до міста {city} не знайдено", ConsoleColor.Yellow);
            return;
        }
        Console.WriteLine($"  Знайдено рейсів з/до міста {city}: {found.Count}");
        PrintTable(found);
    }

    private void Remove()
    {
        string number = ConsoleInput.ReadText("Номер рейсу для вилучення: ");
        if (flights.Remove(number, out Airplane? removed))
        {
            ConsoleInput.WriteColored($"  Remove(\"{number}\") = True - вилучено рейс {removed.DepartureCity} -> " +
                                      $"{removed.ArrivalCity}, Count = {flights.Count}", ConsoleColor.Green);
        }
        else
        {
            ConsoleInput.WriteColored($"  Remove(\"{number}\") = False - рейсу з таким номером немає",
                                      ConsoleColor.Yellow);
        }
    }

    private void ShowCount()
    {
        Console.WriteLine($"  Count = {flights.Count}");
        if (flights.Count == 0)
            return;
        Console.WriteLine($"  Ключі (Keys): {string.Join(", ", flights.Keys)}");
        Console.WriteLine($"  Сумарна дальність рейсів (Values): {FormatKm(flights.Values.Sum(a => a.DistanceKilometers))}");
    }

    private void Clear()
    {
        int count = flights.Count;
        flights.Clear();
        ConsoleInput.WriteColored($"  Clear() - вилучено всі рейси ({count}), Count = {flights.Count}", ConsoleColor.Green);
    }

    private static void PrintTable(IEnumerable<KeyValuePair<string, Airplane>> rows)
    {
        Console.WriteLine($"  {"Номер",-8}{"Маршрут",-24}{"Відправлення",-18}{"Прибуття",-18}{"Дальність",11}  У дорозі");
        Console.WriteLine("  " + new string('-', 92));
        foreach (KeyValuePair<string, Airplane> pair in rows)
        {
            Airplane a = pair.Value;
            string route = $"{a.DepartureCity} -> {a.ArrivalCity}";
            Console.WriteLine($"  {pair.Key,-8}{route,-24}{a.DepartureDate,-18}{a.ArrivalDate,-18}" +
                              $"{FormatKm(a.DistanceKilometers),11}  {FormatTime(a.GetTotalTime())}");
        }
    }

    private static string FormatKm(double kilometers) => $"{kilometers:0.##} км";

    private static string FormatTime(int minutes) => $"{minutes / 60} год {minutes % 60:D2} хв";
}
