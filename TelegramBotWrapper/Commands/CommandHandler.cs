using System;
using System.Collections.Generic;
using Telegram.Bot;
using System.Reflection;
using System.Linq;

namespace TelegramBotWrapper.Commands
{
    public class CommandHandler
    {
        private static IDictionary<CommandMethodAttribute, Action<TelegramBotClient, Command>> _commands = new Dictionary<CommandMethodAttribute, Action<TelegramBotClient, Command>>();
        private TelegramBotClient _bot;

        public CommandHandler(TelegramBotClient bot)
        {
            _bot = bot;
        }

        internal CommandHandler AddCommandContainer<T>()
        {
            return AddCommandContainer(typeof(T));
        }

        internal CommandHandler AddCommandContainer(Type type)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            foreach (MethodInfo method in methods)
            {
                CommandMethodAttribute attribute = method.GetCustomAttribute<CommandMethodAttribute>();
                if (attribute != null)
                {
                    _commands.Add(attribute, (bot, command) =>
                    {
                        method.Invoke(null, new object[] { bot, command });
                    });
                }
            }

            return this;
        }

        private static KeyValuePair<CommandMethodAttribute, Action<TelegramBotClient, Command>>? FindCommand(string identifier)
        {
            var foundCommand = _commands.FirstOrDefault(c => c.Key.Identifier == identifier);
            if (foundCommand.Key != null)
            {
                return foundCommand;
            }

            return null;
        }

        public void Handle(Command command)
        {
            var foundCommand = FindCommand(command.Identifier);
            if (foundCommand.HasValue)
            {
                foundCommand.Value.Value.Invoke(_bot, command);
            }
        }

        public static IList<CommandMethodAttribute> ListAllCommands()
        {
            return _commands.Select(c => c.Key).ToList();
        }

        public static CommandMethodAttribute GetCommandInfo(string type)
        {
            var foundCommand = FindCommand(type);
            if (foundCommand.HasValue)
            {
                return foundCommand.Value.Key;
            }

            return null;
        }
    }
}
