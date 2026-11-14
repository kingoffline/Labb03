using L046_Labb3_Code_along.Models;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace L046_Labb3_Code_along.Services;

internal class QuestionPackStorage
{
    // Gör JSON-filen läsbar och sparar Difficulty som "Easy" i stället för 0.
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public static string FolderPath => AppContext.BaseDirectory;

    public static string FilePath => Path.Combine(FolderPath, "questionpacks.json");

    public async Task<List<QuestionPack>> LoadAsync()
    {
        Directory.CreateDirectory(FolderPath);

        if (!File.Exists(FilePath))
            return new List<QuestionPack>();

        var json = await File.ReadAllTextAsync(FilePath);

        if (string.IsNullOrWhiteSpace(json))
            return new List<QuestionPack>();

        return JsonSerializer.Deserialize<List<QuestionPack>>(json, Options)
               ?? new List<QuestionPack>();
    }

    public async Task SaveAsync(List<QuestionPack> packs)
    {
        Directory.CreateDirectory(FolderPath);

        var json = JsonSerializer.Serialize(packs, Options);

        await File.WriteAllTextAsync(FilePath, json);
    }
}