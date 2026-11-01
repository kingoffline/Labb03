using L046_Labb3_Code_along.Models;
using System.Collections.ObjectModel;

namespace L046_Labb3_Code_along.ViewModels;

internal class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    private Question? _selectedQuestion;

    public QuestionPackViewModel ActivePack => _mainWindowViewModel.ActivePack;
    public ObservableCollection<Question> Questions => ActivePack.Questions;

    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set => SetProperty(ref _selectedQuestion, value);
    }

    public ConfigurationViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }

    public void RaiseActivePackChanged()
    {
        RaisePropertyChanged(nameof(ActivePack));
        RaisePropertyChanged(nameof(Questions));
        if (Questions.Count > 0)
        {
            SelectedQuestion = Questions[0];
        }
        else
        {
            SelectedQuestion = null;
        }
    }
}
