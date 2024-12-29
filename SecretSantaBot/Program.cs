using SecretSantaBot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SecretSantaBot;

class Program {
    private static readonly string _token = /*BOT TOKEN*/;
    static async Task Main(string[] args) {
        var bot = new TelegramBotClient(_token);
        bot.StartReceiving(UpdateHandler, ErrorHandler);
        Console.WriteLine("Bot started!");
        await Task.Delay(-1);
    }

    private static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, HandleErrorSource arg3, CancellationToken arg4) {
        Console.WriteLine("Telegram API error: " + arg2.Message);
        return Task.CompletedTask;
    }

    private static async Task UpdateHandler(ITelegramBotClient arg1, Update arg2, CancellationToken arg3) {
        if (arg2.Message?.Text != null) {
            var userCommandsHandler = new UserCommandsHandler(arg2, arg1);
            await userCommandsHandler.StartRecivingUserCommands();
        }
    }
}
