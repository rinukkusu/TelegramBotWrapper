using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TelegramBotWrapper.Settings;

namespace WeatherPlugin
{
    [DataContract]
    [SettingsPath("./weather.json")]
    class WeatherSettings : SettingsBase<WeatherSettings>
    {
        [DataMember]
        public string ApiToken { get; set; }

        public WeatherSettings()
        {
            ApiToken = "Enter your API token here.";
        }
    }
}
