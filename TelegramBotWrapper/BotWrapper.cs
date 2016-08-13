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

        public event EventHandler<string> OnLog;
        public event EventHandler<MessageEventArgs> OnMessage;

        public BotWrapper()
        {
        }

        public void LogMessage(object sender, string message)
        {
            OnLog?.Invoke(sender, message);
        }

        public void Start()
        {
            LogMessage(this, "Loading settings ...");
            LoadSettings();

            LogMessage(this, "Initializing TelegramBot ...");
            InitBot();

            LogMessage(this, "Initializing command handlers ...");
            InitCommandHandler();

            LogMessage(this, "Initializing events ...");
            InitEvents();

            _bot.StartReceiving();
            LogMessage(this, "=> Ready to roll <=");
        }

        public void Stop()
        {
            _bot.StopReceiving();
        }

        private void LoadSettings()
        {
            bool editSettings = !BotSettings.SettingsExist();

            _settings = BotSettings.Load();

            if (editSettings)
            {
                LogMessage(this, "Please edit the settings.json");
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
            _commandHandler.LoadPlugins();
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
            OnMessage?.Invoke(sender, e);

            if (e.Message.Text != null)
            {
                LogMessage(this, $"[{e.Message.Date.ToString()}] {e.Message.From.Username}: {e.Message.Text}");

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
