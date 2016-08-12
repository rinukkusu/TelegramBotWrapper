using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace TelegramBotWrapper.Settings
{
    [DataContract]
    public abstract class SettingsBase<T> where T : SettingsBase<T>
    {       
        public static bool SettingsExist()
        {
            return File.Exists(GetFilePath());
        }

        public static T Load()
        {
            T settings;

            if (!SettingsExist())
            {
                settings = typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { }) as T;

                settings.Save();
            }
            else
            {
                string settingsString = File.ReadAllText(GetFilePath());
                settings = JsonConvert.DeserializeObject<T>(settingsString);
            }

            return settings;
        }

        public void Save()
        {
            string settingsString = JsonConvert.SerializeObject(this);
            File.WriteAllText(GetFilePath(), settingsString);
        }

        private static string GetFilePath()
        {
            SettingsPathAttribute attribute = typeof(T).GetTypeInfo().GetCustomAttribute<SettingsPathAttribute>();
            return attribute.Path;
        }
    }
}
