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

using System.Windows.Input;
using Alteridem.GitHub.Annotations;
using Alteridem.GitHub.Extension.Interfaces;
using Alteridem.GitHub.Extension.View;
using Octokit;

#endregion

namespace Alteridem.GitHub.Extension.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        private ICommand _loginCommand;

        public UserViewModel()
        {
            LoginCommand = new RelayCommand(Login, p => CanLogin() );
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
            get { return GitHubApi.User; }
        }

        public bool LoggedIn
        {
            get { return GitHubApi.LoggedIn; }
        }

        private void Login(object o)
        {
            if (GitHubApi.LoggedIn)
            {
                GitHubApi.Logout();
                return;
            }

            var dlg = Factory.Get<ILoginView>();
            var view = o as IWindowProvider;
            if ( view != null )
                dlg.Owner = view.Window;
            dlg.ShowDialog();
        }

        private bool CanLogin()
        {
            return true;
        }
    }
}