using System.Text.RegularExpressions;
using Telegram.Bot;

namespace SecretSantaBot.Components;

public static class SecretSanta {
    public static List<T> Shuffle<T>(this List<T> list) {
        var random = new Random();
        for (int i = list.Count - 1; i > 0; i--) {
            int j = random.Next(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
        return list;
    }

    public static List<string> GetChatIds(this string message) {
        var pattern = @"\d+\.\s*(\d+)";
        var regex = new Regex(pattern);
        var ids = new List<string>();
        
        foreach (Match match in regex.Matches(message)) {
            ids.Add(match.Groups[1].Value);
        }

        return ids;
    }
    
    public static async Task<string> GetUsernameByChatId(long chatId, ITelegramBotClient _bot) {
        try {
            var chatMember = await _bot.GetChatMemberAsync(chatId, chatId);
            return chatMember.User.FirstName + $" ({chatMember.User.Username})" ?? "Без імені";
        }
        catch (Exception ex) {
            Console.WriteLine($"Помилка: {ex.Message}");
            return null;
        }
    }
}