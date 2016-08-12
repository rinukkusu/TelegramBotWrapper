using System;
using System.Collections.Generic;
using Telegram.Bot;
using System.Reflection;
using System.Linq;
using System.IO;

namespace TelegramBotWrapper.Commands
{
    public class CommandHandler
    {
        private static IDictionary<CommandInfoAttribute, ICommandContainer> _commandContainers =  new Dictionary<CommandInfoAttribute, ICommandContainer>();

        private TelegramBotClient _bot;

        public CommandHandler(TelegramBotClient bot)
        {
            _bot = bot;

            LoadCommandsFromAssembly(Assembly.GetExecutingAssembly());
        }

        internal void LoadPlugins()
        {
            Directory.GetFiles("./", "*Plugin.dll").ToList().ForEach(plugin =>
            {
                Assembly pluginAssembly = Assembly.LoadFile(Path.GetFullPath(plugin));
                LoadCommandsFromAssembly(pluginAssembly);
            });
        }

        internal void LoadCommandsFromAssembly(Assembly assembly)
        {
            assembly.GetTypes().ToList().ForEach(type =>
            {
                if (type.IsSubclassOf(typeof(CommandContainerBase)))
                {
                    AddCommandContainer(type);
                }
            });
        }

        internal CommandHandler AddCommandContainer<T>()
        {
            return AddCommandContainer(typeof(T));
        }

        internal CommandHandler AddCommandContainer(Type type)
        {
            ICommandContainer commandContainer = type.GetConstructor(new Type[] { typeof(TelegramBotClient) }).Invoke(new object[] { _bot }) as ICommandContainer;
            CommandInfoAttribute attribute = type.GetCustomAttribute<CommandInfoAttribute>();

            _commandContainers.Add(attribute, commandContainer);

            return this;
        }

        private static KeyValuePair<CommandInfoAttribute, ICommandContainer>? FindCommand(string identifier)
        {
            var foundCommand = _commandContainers.FirstOrDefault(c => c.Key.Identifier == identifier);
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
                try
                {
                    bool success = foundCommand.Value.Value.Execute(command);
                    if (!success)
                    {
                        var info = GetCommandInfo(command.Identifier);
                        string returnText = $"Usage:{Environment.NewLine}*/{info.Usage}* - {info.Description}";
                        _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, returnText, false, false, 0, null, Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                }
                catch (Exception ex)
                {
                    _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, ex.Message, false, false, 0, null, Telegram.Bot.Types.Enums.ParseMode.Markdown);
                }
            }
        }

        public static IList<CommandInfoAttribute> ListAllCommands()
        {
            return _commandContainers.Select(c => c.Key).ToList();
        }

        public static CommandInfoAttribute GetCommandInfo(string type)
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
