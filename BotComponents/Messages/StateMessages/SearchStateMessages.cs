using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ValoBotV1.BotComponents.Messages.StateMessages;

public static class SearchStateMessages
{
    public static async Task EnterTacticName(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, "Введите название тактики:",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard);
    }
    
    public static async Task PasteSearchedData(ITelegramBotClient botClient, Context context, string pastingData)
    {
        await botClient.SendTextMessageAsync(context.ChatId, pastingData,
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task PasteSearchedData(ITelegramBotClient botClient, Context context, string pastingData,
        string typeOfKeyboard)
    {
        switch (typeOfKeyboard)
        {
            case "cancel":
                await botClient.SendTextMessageAsync(context.ChatId, pastingData,
                    replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown);
                break;
            case "cancelWithRand":
                await botClient.SendTextMessageAsync(context.ChatId, pastingData,
                    replyMarkup: Keyboards.ReplyKeyboards.CancellationWithRandKeyboard, parseMode: ParseMode.Markdown);
                break;
            default:
                break;
        }
        
    }
}