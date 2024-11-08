using System;
using System.Collections.Generic;

public class FactoryRecord
{
    // Поля для зберігання даних запису
    public string FactoryName { get; set; }
    public double PlannedConsumption { get; set; }
    public double ActualConsumption { get; set; }
    public double Deviation { get; private set; }
    public double PercentageDeviation { get; private set; }

    // Конструктор для ініціалізації запису
    public FactoryRecord(string factoryName, double plannedConsumption, double actualConsumption)
    {
        FactoryName = factoryName;
        PlannedConsumption = plannedConsumption;
        ActualConsumption = actualConsumption;
        CalculateDeviation();
        CalculatePercentageDeviation();
    }

    // Метод для обчислення відхилення (кВт/год)
    public void CalculateDeviation()
    {
        Deviation = PlannedConsumption - ActualConsumption;
    }

    // Метод для обчислення відсоткового відхилення
    public void CalculatePercentageDeviation()
    {
        if (PlannedConsumption != 0)
            PercentageDeviation = (Deviation * 100) / PlannedConsumption;
        else
            PercentageDeviation = 0;  // Уникнення ділення на 0
    }

    // Метод для форматованого виведення даних заводу
    public void PrintRecord()
    {
        Console.WriteLine($"| {FactoryName,-10} | {PlannedConsumption,15:F2} | {ActualConsumption,15:F2} | {Deviation,15:F2} | {PercentageDeviation,10:F2} |");
    }
}

public class EnergyReport
{
    // Список записів (композиція)
    private List<FactoryRecord> records;

    // Конструктор, що створює порожній список записів
    public EnergyReport()
    {
        records = new List<FactoryRecord>();
    }

    // Метод для додавання запису у відомість
    public void AddRecord(FactoryRecord record)
    {
        records.Add(record);
    }

    // Метод для виведення всіх записів
    public void PrintAllRecords()
    {
        Console.WriteLine("\n| Завод      | Планове споживання  | Фактичне споживання | Відхилення (кВт) | Відхилення (%) |");
        Console.WriteLine(new string('-', 80));
        foreach (var record in records)
        {
            record.PrintRecord();
        }
    }

    // Метод для виведення підсумкових значень
    public void PrintTotals()
    {
        double totalPlanned = 0, totalActual = 0, totalDeviation = 0;

        foreach (var record in records)
        {
            totalPlanned += record.PlannedConsumption;
            totalActual += record.ActualConsumption;
            totalDeviation += record.Deviation;
        }

        Console.WriteLine(new string('-', 80));
        Console.WriteLine($"| {"Разом",-10} | {totalPlanned,15:F2} | {totalActual,15:F2} | {totalDeviation,15:F2} | {"-",10} |");
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        // Створення об'єкта класу "ціле" - EnergyReport
        EnergyReport report = new EnergyReport();

        int n = 0;
        while (true)
        {
            try
            {
                Console.Write("Введіть кількість записів: ");
                n = int.Parse(Console.ReadLine());
                if (n <= 0)
                {
                    throw new ArgumentException("Кількість записів повинна бути більше нуля!");
                }
                break;
            }
            catch (FormatException)
            {
                Console.WriteLine("Помилка: введіть правильне ціле число.");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Помилка: {e.Message}");
            }
        }

        // Введення даних для кожного запису
        for (int i = 0; i < n; i++)
        {
            string name;
            double planned = 0, actual = 0;

            // Введення назви заводу
            Console.Write($"Введіть назву заводу для запису {i + 1}: ");
            name = Console.ReadLine();

            // Введення планового споживання з обробкою помилок
            while (true)
            {
                try
                {
                    Console.Write($"Введіть планове споживання для {name}: ");
                    planned = double.Parse(Console.ReadLine());
                    if (planned < 0)
                    {
                        throw new ArgumentException("Планове споживання не може бути від'ємним!");
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Помилка: введіть правильне число.");
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Помилка: {e.Message}");
                }
            }

            // Введення фактичного споживання з обробкою помилок
            while (true)
            {
                try
                {
                    Console.Write($"Введіть фактичне споживання для {name}: ");
                    actual = double.Parse(Console.ReadLine());
                    if (actual < 0)
                    {
                        throw new ArgumentException("Фактичне споживання не може бути від'ємним!");
                    }
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Помилка: введіть правильне число.");
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"Помилка: {e.Message}");
                }
            }

            // Створення нового запису і додавання його у відомість
            report.AddRecord(new FactoryRecord(name, planned, actual));
        }

        // Виведення всіх записів
        report.PrintAllRecords();

        // Виведення підсумкових значень і додавання програми на гітхаб
        report.PrintTotals();
    }
}

