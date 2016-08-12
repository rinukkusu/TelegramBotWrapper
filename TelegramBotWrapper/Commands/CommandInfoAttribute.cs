using System;

namespace TelegramBotWrapper.Commands
{
    public class CommandInfoAttribute : Attribute
    {
        public string Identifier;
        public string Usage;
        public string Description;

        public CommandInfoAttribute(string identifier)
        {
            Identifier = identifier;
        }
    }
}
