using L046_Labb3_Code_along.Command;
using L046_Labb3_Code_along.Models;
using L046_Labb3_Code_along.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace L046_Labb3_Code_along.ViewModels;

internal class MainWindowViewModel : ViewModelBase
{
    private readonly QuestionPackStorage _storage = new();
    private QuestionPackViewModel _activePack = null!;
    private bool _isInitialized;

    public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();
    public ConfigurationViewModel ConfigurationViewModel { get; }

    public event EventHandler? CreateNewPackRequested;
    public event EventHandler? DeletePackRequested;
    public event EventHandler? ImportQuestionsRequested;
    public event EventHandler? PackOptionsRequested;
    public event Action<string>? ErrorRequested;

    public ICommand NewPackCommand { get; }
    public ICommand SelectPackCommand { get; }
    public ICommand DeletePackCommand { get; }
    public ICommand ImportQuestionsCommand { get; }
    public ICommand PackOptionsCommand { get; }

    public ICommand AddQuestionCommand => ConfigurationViewModel.AddQuestionCommand;
    public ICommand RemoveQuestionCommand => ConfigurationViewModel.RemoveQuestionCommand;

    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            if (SetProperty(ref _activePack, value))
            {
                ConfigurationViewModel.RaiseActivePackChanged();
                CommandManager.InvalidateRequerySuggested();
            }
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

        PackOptionsCommand = new RelayCommand(
            _ => PackOptionsRequested?.Invoke(this, EventArgs.Empty),
            _ => ActivePack is not null);

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

    public async Task InitializeAsync()
    {
        if (_isInitialized)
            return;

        _isInitialized = true;

        try
        {
            var loadedPacks = await _storage.LoadAsync();

            if (loadedPacks.Count == 0)
            {
                // Första starten: spara demo-packet så att filen skapas.
                await SaveAsync();
                return;
            }

            Packs.Clear();

            foreach (var pack in loadedPacks)
                Packs.Add(new QuestionPackViewModel(pack));

            ActivePack = Packs[0];
        }
        catch (Exception ex)
        {
            ErrorRequested?.Invoke(
                $"Could not load the saved question packs.\n\n{ex.Message}");
        }
    }

    public async Task SaveAsync()
    {
        try
        {
            var packs = Packs.Select(pack => pack.ToModel()).ToList();
            await _storage.SaveAsync(packs);
        }
        catch (Exception ex)
        {
            ErrorRequested?.Invoke(
                $"Could not save the question packs.\n\n{ex.Message}");
        }
    }

    public void RequestSave()
    {
        _ = SaveAsync();
    }

    public void AddPack(QuestionPack pack)
    {
        var packViewModel = new QuestionPackViewModel(pack);
        Packs.Add(packViewModel);
        ActivePack = packViewModel;
        CommandManager.InvalidateRequerySuggested();
        RequestSave();
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
        RequestSave();
    }
}