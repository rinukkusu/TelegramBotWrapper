using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;

namespace TelegramBotWrapper.Commands
{
    public class Command
    {
        private static readonly string COMMAND_CHARACTER = "/";
        private static readonly Regex _commandRegex = new Regex(@"^/(?<command>.+?)\s(?<arguments>.*)$|^/(?<command_wo>.+?)$");

        public string Identifier { get; private set; }
        public User Sender { get; private set; }
        public IList<string> Arguments { get; private set; }
        public Message OriginalMessage { get; private set; }

        public Command(User sender, string identifier, IList<string> arguments, Message message = null)
        {
            Sender = sender;
            Identifier = identifier;
            Arguments = arguments;
            OriginalMessage = message;
        }

        public static bool IsCommand(string text)
        {
            return text.StartsWith(COMMAND_CHARACTER);
        }

        public static Command FromMessage(Message message)
        {
            Command command = null;

            if (_commandRegex.IsMatch(message.Text))
            {
                Match match = _commandRegex.Match(message.Text);
                string com_wo = match.Groups["command_wo"].Value;
                string com = match.Groups["command"].Value;
                string args = match.Groups["arguments"].Value;

                if (String.IsNullOrWhiteSpace(com))
                    com = com_wo;

                command = new Command(message.From, com, ParseArguments(args), message);
            };

            return command;
        }

        public static IList<string> ParseArguments(string argumentString)
        {
            IList<string> arguments = new List<string>();
            argumentString = argumentString.Trim();

            string singleArgument = "";
            bool bInQuotes = false;
            foreach (char c in argumentString)
            {
                if (c == '\"')
                {
                    bInQuotes = !bInQuotes;
                    continue;
                }

                if (c == ' ')
                {
                    if (bInQuotes)
                    {
                        singleArgument += c.ToString();
                    }
                    else
                    {
                        arguments.Add(singleArgument);
                        singleArgument = "";
                    }
                }
                else
                {
                    singleArgument += c.ToString();
                }
            }

            if (singleArgument.Length > 0)
            {
                arguments.Add(singleArgument);
            }

            return arguments;
        }
    }
}
