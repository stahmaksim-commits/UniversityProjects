using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace DuplicateFileFinder.Benchmarks;

[MemoryDiagnoser]
public class DuplicateFinderBenchmarks
{
    private FileDuplicateService _service = null!;
    private string _testFolderPath = null!;

    [GlobalSetup]
    public void Setup()
    {
        _service = new FileDuplicateService();
        _testFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");
        Directory.CreateDirectory(_testFolderPath);
        
        File.WriteAllText(Path.Combine(_testFolderPath, "file1.txt"), "Test content for benchmark");
        File.WriteAllText(Path.Combine(_testFolderPath, "file2.txt"), "Test content for benchmark"); // дублікат
        File.WriteAllText(Path.Combine(_testFolderPath, "file3.txt"), "Unique content");
    }

    [Benchmark]
    public async Task<Dictionary<string, List<string>>> RunDuplicateSearch()
    {
        return await _service.FindDuplicatesAsync(_testFolderPath);
    }
}