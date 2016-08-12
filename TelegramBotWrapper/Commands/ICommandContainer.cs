using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotWrapper.Commands
{
    interface ICommandContainer
    {
        bool Execute(Command command);
    }
}
