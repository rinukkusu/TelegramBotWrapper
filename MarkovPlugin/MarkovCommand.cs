using LimeBean;
using LimeBean.Interfaces;
using MarkovPlugin.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotWrapper.Commands;

namespace MarkovPlugin
{
    [CommandInfo("m",
        Description = "Markovgenerator",
        Usage = "m <word1> [<word2>]"
    )]
    public class MarkovCommand : CommandContainerBase
    {
        private readonly MarkovPartRepository _markovPartRepository;
        private readonly IBeanAPI _beanApi;

        public MarkovCommand(TelegramBotClient bot) : base(bot)
        {
            _beanApi = new BeanApi("data source=./markov.db", typeof(SqliteConnection));
            _beanApi.EnterFluidMode();

            _markovPartRepository = new MarkovPartRepository(_beanApi);

            _bot.OnMessage += Bot_OnMessage;
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Text != null && !e.Message.Text.StartsWith("/"))
            {
                _markovPartRepository.AddSentence(e.Message.Text.ToLower());
            }
        }

        public override bool Execute(Command command)
        {
            string returnText = "";

            if (command.Arguments.Any())
            {
                try
                {
                    returnText = _markovPartRepository.GetSentence(command.Arguments.First(), command.Arguments.Count > 1 ? command.Arguments[1] : null);
                }
                catch (Exception ex)
                {
                    returnText = "Laberts mal nicht ...";
                }
            }
            else
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(returnText))
            {
                returnText = "Error: Couldn't find any words.";
            }

            _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, returnText);

            return true;
        }
    }
}
