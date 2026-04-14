using Lab4Timp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

using Lab4Timp.Abstraction;
using Lab4Timp.Models;

namespace Lab4Timp.ViewModels
{
    public class AuthorizationWinVM : BaseViewModel
    {
        public string Username { get; set {
                field = value;
                OnPropertyChanged(); 
            } } = "";
        public string Password { get; set {
                field = value;
                OnPropertyChanged();
            } } = "";
        public string InputLanguage { get; private set {
                field = value;
                OnPropertyChanged();
            } } // OneWay
        public bool IsCapsLockOn { get; private set {
                field = value;
                OnPropertyChanged();
            } } // OneWay

        public ICommand Enter { get; }
        public ICommand Cancel { get; }

        private readonly IDialogService _dialogService;
        private readonly IKeyboardStateService _keyboardStateService;
        private readonly IUserLoginer _loginer;

        public AuthorizationWinVM(IDialogService dialogService, IKeyboardStateService keyboardStateService,
            IUserLoginer loginer)
        {
            _dialogService = dialogService;
            _keyboardStateService = keyboardStateService;
            _loginer = loginer;

            Enter = new RelayCommand(() => {
                if (_loginer.Login(Username, Password, out IUserMenuRights userMenuRights))
                {
                    var menuVm = new MenuWinVM(userMenuRights.MenuItems, () => {
                        var authVm = new AuthorizationWinVM(_dialogService, _keyboardStateService, _loginer);
                        _dialogService.ShowWindow(authVm);
                    });
                    _dialogService.ShowWindow(menuVm);
                    _dialogService.CloseWindow(this);
                }
            } );

            Cancel = new RelayCommand(() => {
                _dialogService.CloseWindow(this);
            } );

            InputLanguage = _keyboardStateService.CurrentInputLanguage;
            IsCapsLockOn = _keyboardStateService.IsCapsLockOn;

            _keyboardStateService.InputLanguageChanged += SetLang;
            _keyboardStateService.CapsLockStateChanged += SetCaps;
        }

        private void SetLang()
        {
            InputLanguage = _keyboardStateService.CurrentInputLanguage;
        }

        private void SetCaps()
        {
            IsCapsLockOn = _keyboardStateService.IsCapsLockOn;
        }
    }
}
