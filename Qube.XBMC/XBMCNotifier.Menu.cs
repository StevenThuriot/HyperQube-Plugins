#region License

//  Copyright 2014 Steven Thuriot
//   
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//  http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.

#endregion

using System;
using System.Collections.Generic;
using HyperQube.Library;
using HyperQube.Library.Questions;
using Qube.XBMC.Questions;

namespace Qube.XBMC
{
    partial class XBMCNotifier : IQubeMenuItem
    {
        private const string Ip = "XBMC_IP";
        private const string Port = "XBMC_Port";
        private const string CutAndroidAmount = "XBMC_CutAndroidAmount";
        private const string User = "XBMC_User";
        private const string Password = "XBMC_Password";
        private const string UseHyperIcon = "XBMC_UseHyperIcon";
        private const string HyperIconUri = "XBMC_HyperIconUri";

        private bool _cutAndroidAmount;

        private Uri _hyperIconUri;
        private string _ip;
        private string _port;
        private bool _useHyperIcon;
        private string _user;

        public string Title
        {
            get { return "XMBC Notifier"; }
        }

        public string MenuTitle
        {
            get { return "Settings"; }
        }

        public async void OpenMenu()
        {
            var usernameQuestion = new TextQuestion("Username", initialValue: _user ?? "xbmc");
            var passwordQuestion = new TextQuestion("Password", type: QuestionType.Password);

            var ipQuestion = new IpQuestion(_ip ?? "127.0.0.1");
            var portQuestion = new PortQuestion(_port ?? "9777");
            var cutAndroidAmount = new BooleanQuestion("Format Android-mirrored message.", initialValue: _cutAndroidAmount);

            var useHyperIconQuestion = new BooleanQuestion("Use HyperIcon", initialValue: _useHyperIcon);

            if (_inputProvider.Ask("XBMC Server", "Login and location details.", 
                                        usernameQuestion, passwordQuestion, ipQuestion, 
                                        portQuestion, cutAndroidAmount, useHyperIconQuestion))
            {
                if (useHyperIconQuestion.Result)
                {
                    var initialValue = _hyperIconUri == null ? "http://" + ipQuestion.Result : _hyperIconUri.ToString();
                    var hyperIconServerQuestion = new UriQuestion(initialValue: initialValue);

                    if (_inputProvider.Ask("HyperIcon", "HyperIcon Server Settings.", hyperIconServerQuestion))
                    {
                        _useHyperIcon = true;
                        _hyperIconUri = new Uri(hyperIconServerQuestion.Result);
                    }
                    else
                    {
                        //Cancelled, do nothing;
                        return;
                    }
                }
                else
                {
                    _useHyperIcon = false;
                    _hyperIconUri = null;
                }

                _ip = ipQuestion.Result;
                _port = portQuestion.Result;
                _cutAndroidAmount = cutAndroidAmount.Result;
                _user = usernameQuestion.Result;

                var kvps = new Dictionary<string, string>
                           {
                               {Ip, _ip},
                               {Port, _port},
                               {CutAndroidAmount, _cutAndroidAmount.ToString()},
                               {User, _user},
                               {UseHyperIcon, _useHyperIcon.ToString()}
                           };

                if (!_useHyperIcon || _hyperIconUri == null)
                {
                    kvps.Add(UseHyperIcon, false.ToString());
                }
                else
                {
                    kvps.Add(UseHyperIcon, true.ToString());
                    kvps.Add(HyperIconUri, _hyperIconUri.ToString());
                }

                await _configurationProvider.SetValuesAsync(kvps);
                await _configurationProvider.SetSecureValueAsync(Password, passwordQuestion.Result);
                await _configurationProvider.SaveAsync();
            }
        }







        private async void Initialize()
        {
            var values = await _configurationProvider.GetValuesAsync(Ip, Port, CutAndroidAmount, User, UseHyperIcon, HyperIconUri);

            _ip = values[Ip];
            _port = values[Port];
            bool.TryParse(values[CutAndroidAmount], out _cutAndroidAmount);
            _user = values[User];

            var useHyperIcon = values[UseHyperIcon];

            bool parsedUsage;
            _useHyperIcon = string.IsNullOrWhiteSpace(useHyperIcon) || (bool.TryParse(useHyperIcon, out parsedUsage) && parsedUsage);
            
            Uri uri;
            _hyperIconUri = Uri.TryCreate(values[HyperIconUri], UriKind.Absolute, out uri) ? uri : null;
        }
    }
}
