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
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HyperQube.Library.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Qube.XBMC
{
    partial class XBMCNotifier
    {
        public async Task<string> Build(dynamic json)
        {
            dynamic notification = new JObject();

            notification.id = 1;

            notification.jsonrpc = "2.0";
            notification.method = "GUI.ShowNotification";
            notification.@params = await CreateParameters((object) json);

            return ((JObject) notification).ToString(Formatting.None);
        }

        private async Task<JObject> CreateParameters(object json)
        {
            dynamic parameters = new JObject();

            parameters.title = CreateTitle(json);
            parameters.message = CreateMessage(json);

            if (_useHyperIcon && _hyperIconUri != null)
            {
                var icon = ((dynamic) json).icon;
                if (icon != null)
                {
                    string image = icon.ToString();
                    if (!string.IsNullOrWhiteSpace(image))
                    {
                        using (var webClient = new WebClient())
                        {
                            webClient.UseDefaultCredentials = true;
                            webClient.Proxy = WebRequest.DefaultWebProxy;

                            try
                            {
                                var id = await webClient.UploadStringTaskAsync(_hyperIconUri, image);
                                var imageUri = new Uri(_hyperIconUri, id).ToString();
                                parameters.image = imageUri;
                            }
                            catch (WebException exception)
                            {
                                TraceException(exception);
                            }
                        }
                    }
                }
            }

            return parameters;
        }

        private string CreateMessage(object json)
        {
            var content = Push.GetBody(json);
            if (content == null) return "";

            string body = content.ToString();

            //Removing empty lines;
            IEnumerable<string> values = body.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            if (_cutAndroidAmount && (values.Count() > 1 && values.First().All(char.IsNumber)))
                values = values.Skip(1);

            var message = string.Join("\n", values);
            return message;
        }

        private static string CreateTitle(dynamic json)
        {
            string title = json.title.ToString();

            var appName = json.application_name; //In case of a mirror
            if (appName != null)
            {
                string appString = appName.ToString();
                title += " • " + appString;
            }

            return title;
        }
    }
}
