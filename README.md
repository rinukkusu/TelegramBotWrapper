# TelegramBotWrapper

## Usage

```cs
BotWrapper bot = new BotWrapper();
bot.Start();
```

On the first run a `settings.json` gets created - just edit the ApiToken in there and you're good to go.

## Adding Commands

```cs
[CommandInfo("echo",
    Usage = "echo <text>",
    Description = "This command just echoes a given text."
)]
public class EchoCommand : CommandContainerBase
{
    public EchoCommand(TelegramBotClient bot) : base(bot)
    {
    }
  
    public override bool Execute(Command command) {
        _bot.SendTextMessageAsync(command.OriginalMessage.Chat.Id, command.OriginalMessage.Text);
        
        return true;
    }
}
```
