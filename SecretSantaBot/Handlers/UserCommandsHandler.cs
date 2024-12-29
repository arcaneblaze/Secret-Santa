using SecretSantaBot.Components;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SecretSantaBot.Handlers;

public class UserCommandsHandler {
    private static Update _update;
    private static ITelegramBotClient _bot;

    public UserCommandsHandler(Update update, ITelegramBotClient bot) {
        _update = update;
        _bot = bot;
    }
    
    public async Task StartRecivingUserCommands() {
        await BasicCommands();
        await MenuCommands();
        await TeamCommand();
    }

    private async Task BasicCommands() {
        var inline = new ReplyKeyboardMarkup(new[] {
            new[] {
                new KeyboardButton("👥 Створити команду"),
            },
            new[] {
                new KeyboardButton("🔢 Мій Chat ID")
            }
        }){ ResizeKeyboard = true };
        if (_update.Message.Text == "/start") {
            var greetingMessage = "🎅 Привіт і ласкаво просимо до бота Таємний Санта! 🎄\n\n"
                                     + "🎁 Тут ви зможете організувати незабутній обмін подарунками зі своїми друзями або колегами.\n"
                                     + "✨ Щоб почати, додайте своїх учасників і запустіть жеребкування (кожен з участників повинен натиснути /start в боті).\n\n"
                                     + "🎉 Нехай ця гра стане початком святкового настрою для всіх!";
            await _bot.SendTextMessageAsync(_update.Message.Chat.Id, greetingMessage, replyMarkup:inline);
        }
    }

    private async Task MenuCommands() {
        if (_update.Message.Text.Contains("Створити команду")) {
            string message =
                "👥 Щоб створити команду пропишіть Chat ID кожного з учасників (його можна взяти з головного меню бота).\n"
                + "Мінімальні кількість людей - 2\n"
                + "Нумерація є обов'язковою!\n\n"
                + "Приклад:\n"
                +"/createTeam\n"
                +"1. 71..22\n2. 16..62\n3. 24..60\n";
            await _bot.SendTextMessageAsync(_update.Message.Chat.Id, message);
        }
        
        if (_update.Message.Text.Contains("Мій Chat ID")) {
            await _bot.SendTextMessageAsync(_update.Message.Chat.Id,
                $"Ваш Chat ID: <code>{_update.Message.Chat.Id}</code>", 
                parseMode: ParseMode.Html);
        }
    }

    private async Task TeamCommand() {
        if (_update.Message.Text.StartsWith("/createTeam")) {
            var list = _update.Message.Text.GetChatIds();
        
            if (list.Count < 2) {
                await _bot.SendTextMessageAsync(_update.Message.Chat.Id, 
                    "❌ Помилка! Для створення команди потрібно хоча б два учасника.");
                return;
            }

            list.Shuffle();
            
            for (int i = 0; i < list.Count; i++)
            {
                var giverId = list[i];
                var receiverId = list[(i + 1) % list.Count];
                
                var giverUsername = await SecretSanta.GetUsernameByChatId(long.Parse(giverId), _bot);
                var receiverUsername = await SecretSanta.GetUsernameByChatId(long.Parse(receiverId), _bot);
                
                var message = $"🎁 Вітаємо, вам доручили зробити щасливою людину!\n\n Ви будете дарувати подарунок: {receiverUsername}. 🎄";
                
                await _bot.SendTextMessageAsync(giverId, message);
            }
        }
    }
}