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
    [CommandInfo("b",
        Description = "Edle Bibelverse für Franz und Hans",
        Usage = "b <word>"
    )]
    public class BibleCommand : CommandContainerBase
    {
        private readonly MarkovPartRepository _markovPartRepository;
        private readonly IBeanAPI _beanApi;

        public BibleCommand(TelegramBotClient bot) : base(bot)
        {
            _beanApi = new BeanApi("data source=./bibel.db", typeof(SqliteConnection));
            _beanApi.EnterFluidMode();

            _markovPartRepository = new MarkovPartRepository(_beanApi);
        }

        public override bool Execute(Command command)
        {
            string returnText = "";

            if (command.Arguments.Any())
            {
                try
                {
                    returnText = _markovPartRepository.GetSentence(command.Arguments.First());
                }
                catch (Exception ex)
                {
                    returnText = ex.Message;
                }
            }
            else
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(returnText))
            {
                returnText = "Fehler: Der Herr weiß dazu nichts zu berichten.";
            }

            _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, returnText);

            return true;
        }
    }
}
