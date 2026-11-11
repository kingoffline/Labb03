using L046_Labb3_Code_along.Models;

namespace L046_Labb3_Code_along.ViewModels;

internal class CreateNewPackDialogViewModel : ViewModelBase
{
    private string _name = string.Empty;
    private Difficulty _difficulty = Difficulty.Medium;
    private double _timeLimitInSeconds = 120;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public Difficulty[] Difficulties { get; } = Enum.GetValues<Difficulty>();

    public Difficulty Difficulty
    {
        get => _difficulty;
        set => SetProperty(ref _difficulty, value);
    }

    public double TimeLimitInSeconds
    {
        get => _timeLimitInSeconds;
        set
        {
            if (SetProperty(ref _timeLimitInSeconds, Math.Round(value)))
                RaisePropertyChanged(nameof(TimeLimitText));
        }
    }

    // formaterar om double till en hel sekund
    public string TimeLimitText => $"{(int)_timeLimitInSeconds} seconds";

    // Validate använder en out-parameter för att kunna lämna tillbaka ett felmeddelande samtidigt som den returnerar true/false.
    // Code-behind kan då visa meddelandet utan att själv känna till reglerna.
    public bool Validate(out string error)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            error = "The pack name cannot be empty.";
            return false;
        }

        error = string.Empty;
        return true;
    }

    // CreatePack skapar modellen (QuestionPack), inte en ViewModel. Dialogen lämnar alltså ifrån sig ren data som MainWindowViewModel
    // sedan stoppar in i en QuestionPackViewModel.
    public QuestionPack CreatePack()
        => new QuestionPack(Name.Trim(), Difficulty, (int)_timeLimitInSeconds);
}