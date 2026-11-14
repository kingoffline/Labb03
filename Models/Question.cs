using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace L046_Labb3_Code_along.Models;

internal class Question : INotifyPropertyChanged
{
    private string _query = string.Empty;
    private string _correctAnswer = string.Empty;
    private string[] _incorrectAnswers = new string[3];

    public Question()
    {
    }

    public Question(
        string query,
        string correctAnswer,
        string incorrectAnswer1,
        string incorrectAnswer2,
        string incorrectAnswer3)
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
            var newValue = value ?? string.Empty;
            if (_query == newValue)
                return;

            _query = newValue;
            OnPropertyChanged();
        }
    }

    public string CorrectAnswer
    {
        get => _correctAnswer;
        set
        {
            var newValue = value ?? string.Empty;
            if (_correctAnswer == newValue)
                return;

            _correctAnswer = newValue;
            OnPropertyChanged();
        }
    }

    public string[] IncorrectAnswers
    {
        get => _incorrectAnswers;
        set
        {
            _incorrectAnswers = Normalize(value);
            OnPropertyChanged();
            OnPropertyChanged(nameof(IncorrectAnswer1));
            OnPropertyChanged(nameof(IncorrectAnswer2));
            OnPropertyChanged(nameof(IncorrectAnswer3));
        }
    }

    [JsonIgnore]
    public string IncorrectAnswer1
    {
        get => _incorrectAnswers[0];
        set
        {
            var newValue = value ?? string.Empty;
            if (_incorrectAnswers[0] == newValue)
                return;

            _incorrectAnswers[0] = newValue;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public string IncorrectAnswer2
    {
        get => _incorrectAnswers[1];
        set
        {
            var newValue = value ?? string.Empty;
            if (_incorrectAnswers[1] == newValue)
                return;

            _incorrectAnswers[1] = newValue;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public string IncorrectAnswer3
    {
        get => _incorrectAnswers[2];
        set
        {
            var newValue = value ?? string.Empty;
            if (_incorrectAnswers[2] == newValue)
                return;

            _incorrectAnswers[2] = newValue;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Se till att arrayen alltid har exakt tre platser. Annars kan
    // IncorrectAnswer1/2/3 få IndexOutOfRange när JSON saknar värden.
    private static string[] Normalize(string[]? answers)
    {
        var result = new string[3];

        for (var i = 0; i < result.Length; i++)
        {
            result[i] = answers is not null && i < answers.Length
                ? answers[i] ?? string.Empty
                : string.Empty;
        }

        return result;
    }
}