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
