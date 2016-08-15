using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramBotWrapper.Commands;


namespace TextToSpeechPlugin
{
    [CommandInfo("say",
        Description = "Text to speech.",
        Usage = "say <text>"
    )]
    public class TextToSpeechCommand : CommandContainerBase
    {

        public TextToSpeechCommand(TelegramBotClient bot) : base(bot)
        {

        }

        public override bool Execute(Command command)
        {
            if (command.Arguments.Any())
            {
                string text = String.Join(" ", command.Arguments);

                byte[] speechBytes = TextToSpeech.GetSpeechFromText(text);
                if (speechBytes != null)
                {
                    MemoryStream stream = new MemoryStream(speechBytes);
                    _bot.SendVoiceAsync(command.OriginalMessage.Chat.Id, new Telegram.Bot.Types.FileToSend("audio.wav", stream));
                }
                else
                {
                    _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, "Error: Something went wrong ...");
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
