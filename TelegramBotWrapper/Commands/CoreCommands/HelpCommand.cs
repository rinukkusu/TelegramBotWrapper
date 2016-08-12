using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotWrapper.Commands.Containers
{
    [CommandInfo(
        "help",
        Usage = "help [command]",
        Description = "Lists all commands or shows information about a command."
    )]
    internal class HelpCommand : CommandContainerBase
    {
        public HelpCommand(TelegramBotClient bot) : base(bot)
        {
        }
        
        public override bool Execute(Command command)
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

            _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, returnText, false, false, 0, null, Telegram.Bot.Types.Enums.ParseMode.Markdown);

            return true;
        }
    }
}
