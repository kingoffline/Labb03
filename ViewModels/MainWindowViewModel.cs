using L046_Labb3_Code_along.Command;
using L046_Labb3_Code_along.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace L046_Labb3_Code_along.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private QuestionPackViewModel _activePack = null!;

    public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
    public ConfigurationViewModel ConfigurationViewModel { get; }

    public event EventHandler? CreateNewPackRequested;
    public event EventHandler? DeletePackRequested;
    public event EventHandler? ImportQuestionsRequested;

    public ICommand NewPackCommand { get; }
    public ICommand SelectPackCommand { get; }
    public ICommand DeletePackCommand { get; }
    public ICommand ImportQuestionsCommand { get; }
    public event EventHandler? PackOptionsRequested;

    public ICommand PackOptionsCommand { get; }

    public ICommand AddQuestionCommand => ConfigurationViewModel.AddQuestionCommand;
    public ICommand RemoveQuestionCommand => ConfigurationViewModel.RemoveQuestionCommand;

    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            if (SetProperty(ref _activePack, value))
                ConfigurationViewModel.RaiseActivePackChanged();
        }
    }

    public MainWindowViewModel()
    {
        ConfigurationViewModel = new ConfigurationViewModel(this);

        NewPackCommand = new RelayCommand(_ =>
            CreateNewPackRequested?.Invoke(this, EventArgs.Empty));

        SelectPackCommand = new RelayCommand(parameter =>
        {
            if (parameter is QuestionPackViewModel pack)
                ActivePack = pack;
        });

        DeletePackCommand = new RelayCommand(
            _ => DeletePackRequested?.Invoke(this, EventArgs.Empty),
            _ => Packs.Count > 1);

        ImportQuestionsCommand = new RelayCommand(_ =>
            ImportQuestionsRequested?.Invoke(this, EventArgs.Empty));

        PackOptionsCommand = new RelayCommand(_ => PackOptionsRequested?.Invoke(this, EventArgs.Empty),_ => ActivePack is not null);

        var questionPack = new QuestionPack("Default Question Pack", Difficulty.Easy);
        questionPack.Questions.Add(new Question(
            "In the Morse code, which letter is indicated by 3 dots?",
            "S",
            "O",
            "A",
            "C"));
        questionPack.Questions.Add(new Question(
            "What is the name of sweden's capital?",
            "Stockholm",
            "Göteborg",
            "Malmö",
            "Kalmar"));

        ActivePack = new QuestionPackViewModel(questionPack);
        Packs.Add(ActivePack);
    }

    public void AddPack(QuestionPack pack)
    {
        var packViewModel = new QuestionPackViewModel(pack);
        Packs.Add(packViewModel);
        ActivePack = packViewModel;
        CommandManager.InvalidateRequerySuggested();
    }

    public void DeleteActivePack()
    {
        if (ActivePack is null || Packs.Count <= 1)
            return;

        var index = Packs.IndexOf(ActivePack);
        if (index < 0)
            index = 0;

        Packs.Remove(ActivePack);
        ActivePack = Packs[Math.Min(index, Packs.Count - 1)];
        CommandManager.InvalidateRequerySuggested();
    }
}