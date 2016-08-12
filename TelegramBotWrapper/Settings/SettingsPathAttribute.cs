using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotWrapper.Settings
{
    public class SettingsPathAttribute : Attribute
    {
        public string Path { get; set; }

        public SettingsPathAttribute(string path)
        {
            Path = path;
        }
    }
}
