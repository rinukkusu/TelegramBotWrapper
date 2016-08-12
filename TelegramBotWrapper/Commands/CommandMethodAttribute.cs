using System;

namespace TelegramBotWrapper.Commands
{
    public class CommandMethodAttribute : Attribute
    {
        public string Identifier;
        public string Usage;
        public string Description;
    }
}
