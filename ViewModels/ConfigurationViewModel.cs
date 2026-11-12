using L046_Labb3_Code_along.Command;
using L046_Labb3_Code_along.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace L046_Labb3_Code_along.ViewModels;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    private Question? _selectedQuestion;

    public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;

        AddQuestionCommand = new RelayCommand(
            _ => AddQuestion(),
            _ => ActivePack is not null);

        RemoveQuestionCommand = new RelayCommand(
            _ => RemoveQuestion(),
            _ => SelectedQuestion is not null);
    }

    public QuestionPackViewModel ActivePack => _mainWindowViewModel.ActivePack;
    public ObservableCollection<Question> Questions => ActivePack.Questions;

    public ICommand AddQuestionCommand { get; }
    public ICommand RemoveQuestionCommand { get; }

    // Samma command som MainWindowViewModel äger.
    public ICommand PackOptionsCommand => _mainWindowViewModel.PackOptionsCommand;

    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            if (SetProperty(ref _selectedQuestion, value))
                CommandManager.InvalidateRequerySuggested();
        }
    }

    public void RaiseActivePackChanged()
    {
        RaisePropertyChanged(nameof(ActivePack));
        RaisePropertyChanged(nameof(Questions));

        SelectedQuestion = Questions.Count > 0 ? Questions[0] : null;
        CommandManager.InvalidateRequerySuggested();
    }

    private void AddQuestion()
    {
        if (ActivePack is null)
            return;

        var question = new Question(
            "New question",
            "Correct answer",
            "Incorrect answer 1",
            "Incorrect answer 2",
            "Incorrect answer 3");

        ActivePack.Questions.Add(question);
        SelectedQuestion = question;
        CommandManager.InvalidateRequerySuggested();
    }

    private void RemoveQuestion()
    {
        if (ActivePack is null || SelectedQuestion is null)
            return;

        var index = ActivePack.Questions.IndexOf(SelectedQuestion);
        ActivePack.Questions.Remove(SelectedQuestion);

        SelectedQuestion = ActivePack.Questions.Count == 0
            ? null
            : ActivePack.Questions[Math.Min(index, ActivePack.Questions.Count - 1)];

        CommandManager.InvalidateRequerySuggested();
    }
}