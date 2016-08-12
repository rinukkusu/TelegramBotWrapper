using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBotWrapper.Commands;
using TelegramBotWrapper.Settings;

namespace TelegramBotWrapper
{
    public class BotWrapper
    {
        private TelegramBotClient _bot { get; set; }
        private CommandHandler _commandHandler { get; set; }
        private BotSettings _settings { get; set; }
        
        public BotWrapper()
        {
            LoadSettings();

            InitBot();

            InitCommandHandler();
            InitEvents();
        }

        private void LoadSettings()
        {
            bool editSettings = !BotSettings.SettingsExist();

            _settings = BotSettings.Load();

            if (editSettings)
            {
                Console.WriteLine("Please edit the settings.json");
                Environment.Exit(-1);
            }
        }

        private void InitBot()
        {
            _bot = new TelegramBotClient(_settings.ApiToken);
        }

        private void InitCommandHandler()
        {
            _commandHandler = new CommandHandler(_bot);
            var typesWithMyAttribute =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(CommandContainerAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<CommandContainerAttribute>() };

            typesWithMyAttribute.ForAll(container =>
            {
                _commandHandler.AddCommandContainer(container.Type);
            });
        }

        private void InitEvents()
        {
            _bot.OnMessage += Bot_OnMessage;
            _bot.OnUpdate += Bot_OnUpdate;
        }

        private void HandleCommand(Command command)
        {
            _commandHandler.Handle(command);
        }

        private void Bot_OnUpdate(object sender, UpdateEventArgs e)
        {
            // TODO
        }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"[{e.Message.Date.ToString()}] {e.Message.From.Username}: {e.Message.Text}");

                if (Command.IsCommand(e.Message.Text))
                {
                    Command command = Command.FromMessage(e.Message);
                    if (command != null)
                    {
                        HandleCommand(command);
                    }
                }
            }
        }
    }
}
