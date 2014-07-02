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
using System.ComponentModel.Composition;
using System.Globalization;
using System.Net;
using HyperQube.Library;

namespace Qube.XBMC
{
    // ReSharper disable once InconsistentNaming
    partial class XBMCNotifier : IQube
    {
        private readonly IInputProvider _inputProvider;
        private readonly IOutputProvider _outputProvider;
        private readonly IConfigurationProvider _configurationProvider;

        [ImportingConstructor]
        public XBMCNotifier(IInputProvider inputProvider, IOutputProvider outputProvider, IConfigurationProvider configurationProvider)
        {
            _inputProvider = inputProvider;
            _outputProvider = outputProvider;
            _configurationProvider = configurationProvider;

            Initialize();
        }

        public Interests Interests
        {
            get { return Interests.All; }
        }

        public async void Receive(dynamic json)
        {
            if (string.IsNullOrEmpty(_ip) || string.IsNullOrEmpty(_port) || string.IsNullOrEmpty(_user))
            {
                _outputProvider.Write("XBMC Notifier", "Server is not configured correctly.", QubeIcon.Error);
                return;
            }

            string title = json.title;
            if (title == null || title.StartsWith("QUBE;", StringComparison.OrdinalIgnoreCase))
                return; //Ignore HyperQube triggers.

            var notificationAsJson = await Build((object) json);

            try
            {
                await SendNotification(notificationAsJson);
            }
            catch (WebException exception)
            {
                var message = string.Format(CultureInfo.CurrentCulture, "{0} ( {1} )", exception.Message, exception.Status);

                _outputProvider.TraceError(message);
                _outputProvider.Write("XBMC Server", message, QubeIcon.Error);
            }
        }
    }
}
