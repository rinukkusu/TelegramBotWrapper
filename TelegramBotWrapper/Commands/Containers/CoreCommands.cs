using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotWrapper.Commands.Containers
{
    [CommandContainer]
    internal class CoreCommands
    {
        [CommandMethod(
            Identifier = "help",
            Usage = "help [command]",
            Description = "Lists all commands or shows information about a command."
        )]
        public static void Help(TelegramBotClient bot, Command command)
        {
            string returnText = "";

            if (command.Arguments.Any())
            {
                var info = CommandHandler.GetCommandInfo(command.Arguments.First());
                returnText = $"*Usage:* /{info.Usage}{Environment.NewLine}" +
                             $"*Description:* {info.Description}";

            }
            else
            {
                var commands = CommandHandler.ListAllCommands();

                returnText = $"All Commands:{Environment.NewLine}{Environment.NewLine}";

                foreach (var info in commands)
                {
                    returnText += $"*/{info.Usage}* - {info.Description}{Environment.NewLine}";
                }
            }

            bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, returnText, false, false, 0, null, Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }
    }
}
