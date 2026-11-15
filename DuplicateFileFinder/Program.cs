namespace DuplicateFileFinder;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Duplicate File Finder ===");
        
        string path = args.Length > 0 ? args[0] : string.Empty;

        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            Console.Write("Введіть шлях до папки для сканування: ");
            path = Console.ReadLine()?.Trim() ?? string.Empty;
        }

        var finder = new FileDuplicateService();
        Console.WriteLine($"\nСканування папки: {path}...");

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            var duplicates = await finder.FindDuplicatesAsync(path);
            stopwatch.Stop();

            Console.WriteLine($"\nСканування завершено за {stopwatch.ElapsedMilliseconds} мс.");
            
            if (duplicates.Count == 0)
            {
                Console.WriteLine("Дублікатів не знайдено.");
            }
            else
            {
                Console.WriteLine($"Знайдено груп дублікатів: {duplicates.Count}\n");
                int groupNum = 1;
                foreach (var dup in duplicates)
                {
                    Console.WriteLine($"Група #{groupNum++} (Хеш: {dup.Key[..8]}...):");
                    foreach (var file in dup.Value)
                    {
                        Console.WriteLine($"   - {file}");
                    }
                    Console.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }
}