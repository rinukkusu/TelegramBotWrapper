using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotWrapper.Commands;
using WeatherPlugin.Models;

namespace WeatherPlugin
{
    [CommandInfo("w",
        Usage = "w <place>",
        Description = "Gathers weather information."
    )]
    public class WeatherCommand : CommandContainerBase
    {
        private WeatherSettings _settings;
        private static string API_URL = "http://api.openweathermap.org/data/2.5/weather?units=metric";

        public WeatherCommand(TelegramBotClient bot) : base(bot)
        {
            _settings = WeatherSettings.Load();
        }
        
        public override bool Execute(Command command)
        {
            string returnText = "";

            if (command.Arguments.Any())
            {
                string place = command.Arguments.First().Replace(" ", ",");

                string returnstring = $"%CITY% | %TEMP%°C | %WEATHERINFO% | H: %HUMIDITY%%, P: %PRESSURE%hPa";

                string ApiUrl = "http://api.openweathermap.org/data/2.5/weather?units=metric&APPID=f2deb5b7695a3f73da3e353c8d4bffd9&q=";

                byte[] jsonbytes = new byte[] { };

                WebClient client = new WebClient();
                try
                {
                    jsonbytes = client.DownloadData(ApiUrl + place);
                }
                catch (Exception ex)
                {
                    returnText = ex.Message;
                }

                string jsonResponse = UTF8Encoding.UTF8.GetString(jsonbytes);

                IResponse response;

                try
                {
                    response = JsonConvert.DeserializeObject<WeatherResponse>(jsonResponse);
                }
                catch
                {
                    response = JsonConvert.DeserializeObject<ErrorResponse>(jsonResponse);
                }

                if (response != null)
                {
                    if (response.cod == 200)
                    {
                        var wResponse = ((WeatherResponse)response);

                        returnstring = returnstring.Replace("%CITY%", wResponse.name);
                        returnstring = returnstring.Replace("%TEMP%", wResponse.main.temp.ToString());
                        returnstring = returnstring.Replace("%HUMIDITY%", wResponse.main.humidity.ToString());
                        returnstring = returnstring.Replace("%PRESSURE%", wResponse.main.pressure.ToString());

                        string weatherinfo = String.Empty;
                        foreach (WeatherResponse.Weather info in wResponse.weather)
                        {
                            weatherinfo += info.description + ", ";
                        }
                        weatherinfo = weatherinfo.Trim(' ');
                        weatherinfo = weatherinfo.Trim(',');

                        returnstring = returnstring.Replace("%WEATHERINFO%", weatherinfo);
                    }
                    else
                    {
                        response = JsonConvert.DeserializeObject<ErrorResponse>(jsonResponse);
                        returnstring = ((ErrorResponse)response).message;
                    }

                    returnText = returnstring;
                }

            }
            else
            {
                return false;
            }

            _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, returnText, false, false, 0, null, Telegram.Bot.Types.Enums.ParseMode.Markdown);

            return true;
        }

        private string GetApiUrl(string place)
        {
            return $"{API_URL}&q={place}&APPID={_settings.ApiToken}";
        }
    }
}
