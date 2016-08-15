using LimeBean;
using LimeBean.Interfaces;
using MarkovPlugin.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotWrapper.Commands;

namespace MarkovPlugin
{
    [CommandInfo("bsay",
        Description = "Bibelverse, vorgelesen.",
        Usage = "bsay <word>"
    )]
    public class BibleTTSCommand : CommandContainerBase
    {
        public BibleTTSCommand(TelegramBotClient bot) : base(bot)
        {
        }

        public override bool Execute(Command command)
        {
            string returnText = "";

            if (command.Arguments.Any())
            {
                try
                {
                    returnText = BibleCommand._markovPartRepository.GetSentence(command.Arguments.First());
                    byte[] speechBytes  = TextToSpeechPlugin.TextToSpeech.GetSpeechFromText(returnText);

                    if (speechBytes != null)
                    {
                        MemoryStream stream = new MemoryStream(speechBytes);
                        _bot.SendVoiceAsync(command.OriginalMessage.Chat.Id, new Telegram.Bot.Types.FileToSend("audio.wav", stream));
                    }
                    else
                    {
                        _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, "Error: Something went wrong ...");
                    }

                    return true;
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
