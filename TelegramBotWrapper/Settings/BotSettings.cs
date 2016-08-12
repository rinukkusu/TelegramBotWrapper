using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotWrapper.Settings
{
    [DataContract]
    [SettingsPath("./settings.json")]
    internal class BotSettings : SettingsBase<BotSettings>
    {
        [DataMember]
        public string ApiToken { get; set; }

        public BotSettings()
        {
            ApiToken = "Enter your API token here.";
        }
    }
}
