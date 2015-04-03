using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Qube.XBMC
{
    partial class XBMCNotifier
    {
        private async Task SendNotification(string notificationAsJson)
        {
            var uri = string.Format(CultureInfo.CurrentCulture, "http://{0}:{1}/jsonrpc", _ip, _port);
            var xbmcUri = new Uri(uri);

            string response;
            using (var webclient = new WebClient())
            {
                var encoding = Encoding.UTF8;
                byte[] byteResponse;

                using (var password = await _configurationProvider.GetSecureValueAsync(Password))
                {
                    webclient.Credentials = new NetworkCredential(_user, password);
                    webclient.Headers[HttpRequestHeader.ContentType] = "application/json";

                    byteResponse = await webclient.UploadDataTaskAsync(xbmcUri, encoding.GetBytes(notificationAsJson));
                }

                response = encoding.GetString(byteResponse);
            }

            _outputProvider.Trace(response);
        }
    }
}
