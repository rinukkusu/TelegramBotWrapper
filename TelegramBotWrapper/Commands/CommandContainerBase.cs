using System;
using Telegram.Bot;

namespace TelegramBotWrapper.Commands
{
    public abstract class CommandContainerBase : ICommandContainer
    {
        protected readonly TelegramBotClient _bot;

        public CommandContainerBase(TelegramBotClient bot)
        {
            _bot = bot;
        }

        public abstract bool Execute(Command command);
    }
}
