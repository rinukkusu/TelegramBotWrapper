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

        [DataMember]
        public IDictionary<int, string> UserPlaceMap { get; set; }

        public WeatherSettings()
        {
            ApiToken = "Enter your API token here.";
            UserPlaceMap = new Dictionary<int, string>();
        }

        public string GetPlaceForUser(int user)
        {
            var mapping = UserPlaceMap.FirstOrDefault(kv => kv.Key == user);
            if (mapping.Key != 0)
            {
                return mapping.Value;
            }

            return null;
        }

        public void SetPlaceForUser(int user, string place)
        {
            if (UserPlaceMap == null)
            {
                UserPlaceMap = new Dictionary<int, string>();
            }

            var mapping = UserPlaceMap.FirstOrDefault(kv => kv.Key == user);
            if (mapping.Key == 0)
            {
                UserPlaceMap.Add(user, place);
            }
            else
            {
                UserPlaceMap[user] = place;
            }

            Save();
        }
    }
}
