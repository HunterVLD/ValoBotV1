using Telegram.Bot;

namespace ValoBotV1.BotComponents.Messages;

public static class CommandMessages
{
    public static async Task StartCommand(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(chatId, "Используй кнопки! 📲", 
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task InfoCommand(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(chatId, Constants.CommandInformation[1], 
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
}