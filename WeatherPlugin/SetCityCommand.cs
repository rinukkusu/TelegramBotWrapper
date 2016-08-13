using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotWrapper.Commands;

namespace WeatherPlugin
{
    [CommandInfo("wset",
        Usage = "wset <place>",
        Description = "Sets your personal place for simpler weather retrieval."
    )]
    class SetCityCommand : CommandContainerBase
    {
        public SetCityCommand(TelegramBotClient bot) : base(bot)
        {
        }

        public override bool Execute(Command command)
        {
            if (command.Arguments.Any())
            {
                var place = command.Arguments.First();

                WeatherCommand._settings.SetPlaceForUser(command.Sender.Id, place);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
