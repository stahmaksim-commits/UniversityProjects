using System.Security.Cryptography;

namespace DuplicateFileFinder;

public class FileDuplicateService
{
    public async Task<Dictionary<string, List<string>>> FindDuplicatesAsync(string rootPath)
    {
        if (!Directory.Exists(rootPath))
        {
            throw new DirectoryNotFoundException($"Шлях не знайдено: {rootPath}");
        }

        // Крок 1: Отримуємо всі файли рекурсивно O(N)
        var allFiles = Directory.GetFiles(rootPath, "*.*", SearchOption.AllDirectories);

        // Крок 2: Групуємо за розміром файлу (швидкий фільтр)
        var filesBySize = allFiles
            .Select(f => new FileInfo(f))
            .Where(f => f.Exists)
            .GroupBy(f => f.Length)
            .Where(g => g.Count() > 1) // Відкидаємо унікальні за розміром файли
            .SelectMany(g => g)
            .ToList();

        var hashGroups = new Dictionary<string, List<string>>();

        // Крок 3: Хешуємо вміст тільки для тих, у кого однаковий розмір
        foreach (var file in filesBySize)
        {
            string hash = await ComputeFileHashAsync(file.FullName);
            
            if (!hashGroups.ContainsKey(hash))
            {
                hashGroups[hash] = new List<string>();
            }
            hashGroups[hash].Add(file.FullName);
        }

        // Залишаємо лише ті хеші, де є реальні дублікати (>1 файлу)
        var duplicates = hashGroups
            .Where(g => g.Value.Count > 1)
            .ToDictionary(g => g.Key, g => g.Value);

        return duplicates;
    }

    private async Task<string> ComputeFileHashAsync(string filePath)
    {
        using var sha256 = SHA256.Create();
        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        var hashBytes = await sha256.ComputeHashAsync(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }
}