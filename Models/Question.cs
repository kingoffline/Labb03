using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace L046_Labb3_Code_along.Models;

internal class Question : INotifyPropertyChanged
{
    private string _query;
    private string _correctAnswer;

    public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
    {
        _query = query;
        _correctAnswer = correctAnswer;
        IncorrectAnswers = new[] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
    }

    public string Query
    {
        get => _query;
        set
        {
            if (_query == value)
                return;

            _query = value;
            OnPropertyChanged();
        }
    }

    public string CorrectAnswer
    {
        get => _correctAnswer;
        set
        {
            if (_correctAnswer == value)
                return;

            _correctAnswer = value;
            OnPropertyChanged();
        }
    }

    public string[] IncorrectAnswers { get; set; }

    public string IncorrectAnswer1
    {
        get => IncorrectAnswers[0];
        set
        {
            IncorrectAnswers[0] = value;
            OnPropertyChanged();
        }
    }

    public string IncorrectAnswer2
    {
        get => IncorrectAnswers[1];
        set
        {
            IncorrectAnswers[1] = value;
            OnPropertyChanged();
        }
    }

    public string IncorrectAnswer3
    {
        get => IncorrectAnswers[2];
        set
        {
            IncorrectAnswers[2] = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
