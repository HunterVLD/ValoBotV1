using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ValoBotV1.BotComponents.Messages.StateMessages;

public static class TaggerActionMessages
{
    public static async Task EnterTaggMessage(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, 
            "Введите ваш тэг:\n\n*P.S Нужно вводить пользователей через символ |@|*",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown);
    }
    
    public static async Task SetTagComplete(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, 
            "Вы успешно ввели свой новый тэг! ✅",
            replyMarkup: Keyboards.ReplyKeyboards.SecondKeyboardMarkup);
    }
    
    public static async Task PasteTag(ITelegramBotClient botClient, Context context, string data)
    {
        await botClient.SendTextMessageAsync(context.ChatId, 
            data, replyMarkup: Keyboards.ReplyKeyboards.SecondKeyboardMarkup);
    }
}