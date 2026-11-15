using L046_Labb3_Code_along.Command;
using L046_Labb3_Code_along.Models;
using System.Windows.Input;
using System.Windows.Threading;

namespace L046_Labb3_Code_along.ViewModels;

internal class PlayerViewModel : ViewModelBase
{
    // Litet svarsalternativ – ingen egen ViewModel.
    public sealed record AnswerOption(
        string Text,
        bool IsCorrect,
        bool ShowCheck,
        bool ShowCross);

    private readonly Random _random = new();
    private readonly List<Question> _questions = new();
    private readonly DispatcherTimer _timer;
    private readonly DispatcherTimer _advanceTimer;

    private string _packName = string.Empty;
    private string _currentQuestionText = string.Empty;
    private List<AnswerOption> _answerOptions = new();
    private string _feedbackText = string.Empty;
    private int _currentIndex;
    private int _score;
    private int _timeRemaining;
    private int _timeLimit = 30;
    private bool _hasAnswered;
    private bool _isFinished;

    public PlayerViewModel()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += Timer_Tick;

        // Väntar 2 sekunder efter feedbacken innan nästa fråga.
        _advanceTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
        _advanceTimer.Tick += AdvanceTimer_Tick;

        SubmitAnswerCommand = new RelayCommand(
            parameter => SubmitAnswer(parameter as AnswerOption));
    }

    public string PackName
    {
        get => _packName;
        private set => SetProperty(ref _packName, value);
    }

    public string CurrentQuestionText
    {
        get => _currentQuestionText;
        private set => SetProperty(ref _currentQuestionText, value);
    }

    public List<AnswerOption> AnswerOptions
    {
        get => _answerOptions;
        private set => SetProperty(ref _answerOptions, value);
    }

    public string QuestionProgress => _questions.Count == 0
        ? string.Empty
        : $"Question {_currentIndex + 1} of {_questions.Count}";

    public int TimeRemaining
    {
        get => _timeRemaining;
        private set => SetProperty(ref _timeRemaining, value);
    }

    public int TimeLimit
    {
        get => _timeLimit;
        private set => SetProperty(ref _timeLimit, value);
    }

    public bool HasAnswered
    {
        get => _hasAnswered;
        private set
        {
            if (SetProperty(ref _hasAnswered, value))
                CommandManager.InvalidateRequerySuggested();
        }
    }

    public bool IsFinished
    {
        get => _isFinished;
        private set
        {
            if (SetProperty(ref _isFinished, value))
            {
                _timer.Stop();
                _advanceTimer.Stop();
                RaisePropertyChanged(nameof(ResultText));
            }
        }
    }

    public bool IsPlaying => !_isFinished;

    public string FeedbackText
    {
        get => _feedbackText;
        private set => SetProperty(ref _feedbackText, value);
    }

    public string ResultText => $"You got {_score} out of {_questions.Count} answers correct!";

    public ICommand SubmitAnswerCommand { get; }

    public void StartRound(QuestionPackViewModel pack)
    {
        _timer.Stop();
        _advanceTimer.Stop();
        _questions.Clear();

        var questions = pack.Questions.ToList();
        Shuffle(questions);
        _questions.AddRange(questions);

        PackName = pack.Name;
        TimeLimit = Math.Max(1, pack.TimeLimitInSeconds);
        _score = 0;
        _currentIndex = 0;

        IsFinished = false;
        LoadCurrentQuestion();
    }

    public void StopRound()
    {
        _timer.Stop();
        _advanceTimer.Stop();
    }

    private void LoadCurrentQuestion()
    {
        _timer.Stop();
        _advanceTimer.Stop();

        var question = _questions[_currentIndex];

        CurrentQuestionText = question.Query;
        HasAnswered = false;
        FeedbackText = string.Empty;
        TimeRemaining = TimeLimit;

        var answers = new List<AnswerOption>
        {
            new(question.CorrectAnswer, true, false, false),
            new(question.IncorrectAnswers[0], false, false, false),
            new(question.IncorrectAnswers[1], false, false, false),
            new(question.IncorrectAnswers[2], false, false, false)
        };

        Shuffle(answers);
        AnswerOptions = answers;

        RaisePropertyChanged(nameof(QuestionProgress));

        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (HasAnswered || IsFinished)
        {
            _timer.Stop();
            return;
        }

        TimeRemaining--;

        if (TimeRemaining <= 0)
        {
            TimeRemaining = 0;
            SubmitAnswer(null);
        }
    }

    private void SubmitAnswer(AnswerOption? selected)
    {
        if (HasAnswered || IsFinished)
            return;

        _timer.Stop();
        HasAnswered = true;

        var correctText = AnswerOptions.First(option => option.IsCorrect).Text;

        AnswerOptions = AnswerOptions
            .Select(option => option with
            {
                ShowCheck = option.IsCorrect,
                ShowCross = !option.IsCorrect && ReferenceEquals(option, selected)
            })
            .ToList();

        if (selected is null)
        {
            FeedbackText = $"Time is up! The correct answer was: {correctText}";
        }
        else if (selected.IsCorrect)
        {
            _score++;
            FeedbackText = "Correct!";
        }
        else
        {
            FeedbackText = $"Wrong! The correct answer was: {correctText}";
        }

        // Feedbacken visas i 2 sekunder, sedan går vi vidare automatiskt.
        _advanceTimer.Start();
    }

    private void AdvanceTimer_Tick(object? sender, EventArgs e)
    {
        _advanceTimer.Stop();

        if (IsFinished)
            return;

        if (_currentIndex < _questions.Count - 1)
        {
            _currentIndex++;
            LoadCurrentQuestion();
        }
        else
        {
            _timer.Stop();
            IsFinished = true;
        }
    }

    private void Shuffle<T>(IList<T> items)
    {
        for (var i = items.Count - 1; i > 0; i--)
        {
            var j = _random.Next(i + 1);
            (items[i], items[j]) = (items[j], items[i]);
        }
    }
}