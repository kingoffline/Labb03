using L046_Labb3_Code_along.Models;
using System.Collections.ObjectModel;

namespace L046_Labb3_Code_along.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private QuestionPackViewModel _activePack = null!;
    public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
    public ConfigurationViewModel ConfigurationViewModel { get; }

    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            if (SetProperty(ref _activePack, value))
            {
                ConfigurationViewModel.RaiseActivePackChanged();
            }
        }
    }

    public MainWindowViewModel()
    {
        ConfigurationViewModel = new ConfigurationViewModel(this);

        var questionPack = new QuestionPack("Default Question Pack", Difficulty.Easy);
        questionPack.Questions.Add(new Question(
            "In the Morse code, which letter is indicated by 3 dots?",
            "S",
            "O",
            "A",
            "C"));
        questionPack.Questions.Add(new Question("What is the name of sweden's capital?", "Stockholm", "Göteborg", "Malmö", "Kalmar"));

        ActivePack = new QuestionPackViewModel(questionPack);
        Packs.Add(ActivePack);

    }
}
