﻿// **********************************************************************************
// The MIT License (MIT)
// 
// Copyright (c) 2014 Rob Prouse
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// **********************************************************************************

#region Using Directives

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using Alteridem.GitHub.Annotations;
using Alteridem.GitHub.Extension.View;
using Alteridem.GitHub.Extensions;
using Alteridem.GitHub.Model;
using EnvDTE;
using Octokit;

#endregion

namespace Alteridem.GitHub.Extension.ViewModel
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private User _user;
        private GitHubApi _github;
        private Control _control;
        private ICommand _loginCommand;

        public UserViewModel(Control control)
        {
            _control = control;
            LoginCommand = new RelayCommand(p => Login(), p => CanLogin() );
            _github = Factory.Get<GitHubApi>();
            _github.PropertyChanged += OnGitHubPropertyChanged;
        }

        [NotNull]
        public ICommand LoginCommand
        {
            get { return _loginCommand; }
            set
            {
                if (Equals(value, _loginCommand)) return;
                _loginCommand = value;
                OnPropertyChanged();
            }
        }

        public User User
        {
            get { return _github.User; }
        }

        public bool LoggedIn
        {
            get { return _github.LoggedIn; }
        }

        private void Login()
        {
            if (_github.LoggedIn)
            {
                _github.Logout();
                return;
            }
            var view = Factory.Get<LoginDialog>();
            view.Owner = _control.GetParentWindow();
            view.ShowDialog();
        }

        private bool CanLogin()
        {
            return true;
        }

        private void OnGitHubPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Our properties are named the same, so just chain them
            OnPropertyChanged(e.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}