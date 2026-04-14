using Lab4Timp.Abstraction;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

public class WpfKeyboardStateService : IKeyboardStateService
{
    private readonly DispatcherTimer _timer;
    private readonly Dispatcher _dispatcher;

    public string CurrentInputLanguage { get; private set; }
    public bool IsCapsLockOn { get; private set; }

    public event Action? InputLanguageChanged;
    public event Action? CapsLockStateChanged;

    public WpfKeyboardStateService()
    {
        _dispatcher = Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

        _dispatcher.Invoke(() =>
        {
            InputLanguageManager.Current.InputLanguageChanged += OnInputLanguageChanged;
            UpdateLanguage();
            UpdateCapsLock();
        });

        _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Background, OnTimerTick, _dispatcher);
        _timer.Start();
    }

    private void OnInputLanguageChanged(object? sender, InputLanguageEventArgs e) => UpdateLanguage();

    private void OnTimerTick(object? sender, EventArgs e)
    {
        UpdateLanguage();
        UpdateCapsLock();
    }

    private void UpdateLanguage()
    {
        var culture = InputLanguageManager.Current.CurrentInputLanguage;
        var langName = culture?.TwoLetterISOLanguageName?.ToUpperInvariant() ?? "??";
        if (CurrentInputLanguage != langName)
        {
            CurrentInputLanguage = langName;
            InputLanguageChanged?.Invoke();
        }
    }

    private void UpdateCapsLock()
    {
        bool isCaps = Keyboard.GetKeyStates(Key.CapsLock).HasFlag(KeyStates.Toggled);
        if (IsCapsLockOn != isCaps)
        {
            IsCapsLockOn = isCaps;
            CapsLockStateChanged?.Invoke();
        }
    }
}