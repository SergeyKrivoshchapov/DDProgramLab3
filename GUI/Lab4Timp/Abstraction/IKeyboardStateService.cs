using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4Timp.Abstraction
{
    public interface IKeyboardStateService
    {
        string CurrentInputLanguage { get; }   // "RU", "EN" и т.п.
        bool IsCapsLockOn { get; }

        event Action? InputLanguageChanged;
        event Action? CapsLockStateChanged;
    }
}
