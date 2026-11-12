using L046_Labb3_Code_along.Models;

namespace L046_Labb3_Code_along.ViewModels;

internal class PackOptionsDialogViewModel : ViewModelBase
{
    private string _name;
    private Difficulty _difficulty;
    private double _timeLimitInSeconds;

    public PackOptionsDialogViewModel(QuestionPackViewModel pack)
    {
        _name = pack.Name;
        _difficulty = pack.Difficulty;
        _timeLimitInSeconds = pack.TimeLimitInSeconds;
    }

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

    public string TimeLimitText => $"{(int)_timeLimitInSeconds} seconds";

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

    public void ApplyTo(QuestionPackViewModel pack)
    {
        pack.Name = Name.Trim();
        pack.Difficulty = Difficulty;
        pack.TimeLimitInSeconds = (int)Math.Round(_timeLimitInSeconds);
    }
}