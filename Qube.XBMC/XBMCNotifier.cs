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
                TraceException(exception);
            }
        }

        private void TraceException(WebException ex)
        {
            _outputProvider.TraceError(ex.ToString());

            var message = string.Format(CultureInfo.CurrentCulture, "{0} ( {1} )", ex.Message, ex.Status);
            _outputProvider.Write("XBMC Server", message, QubeIcon.Error);
        }
    }
}
