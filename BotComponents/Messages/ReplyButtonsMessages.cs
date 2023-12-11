using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ValoBotV1.BotComponents.Messages;

public static class ReplyButtonsMessages
{
    public static async Task<int> CreateTactic(ITelegramBotClient botClient, long chatId)
    {
        return (await botClient.SendTextMessageAsync(chatId, 
            "Введите название тактики:\n\n*P.S Ваши пробелы будут автоматически удалены, как и " +
            "другие разделительные символы.*", 
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, 
            parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task<int> EditCreateTactic(ITelegramBotClient botClient, long chatId, string data)
    {
        return (await botClient.SendTextMessageAsync(chatId, 
            $"Введите название тактики:\n\nВы ввели *{data}* ✅", 
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, 
            parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task<int> SearchTactic(ITelegramBotClient botClient, long chatId)
    {
        return (await botClient.SendTextMessageAsync(chatId, "Выберите фильтр для поиска тактики:",
            replyMarkup: Keyboards.InlineKeyboards.SearchFilterKeyboard)).MessageId;
    }
    
    public static async Task<int> EditTactic(ITelegramBotClient botClient, long chatId)
    {
        return (await botClient.SendTextMessageAsync(chatId, "Введите название вашей тактики:", 
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard)).MessageId;
    }
    
    public static async Task<int> DeleteTactic(ITelegramBotClient botClient, long chatId)
    {
        return (await botClient.SendTextMessageAsync(chatId,
            "Введите название тактик(и), которые(ую) хотите удалить:\n\n" +
            "*P.S Вводить больше чем одну тактику нужно через пробел, запятую или точку*",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, 
            parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task CancelAction(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(chatId, "Вы успешно отменили задачу! ⭕",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task<int> TaggerWithAddingTag(ITelegramBotClient botClient, long chatId)
    {
        return (await botClient.SendTextMessageAsync(chatId, "Выберите опцию:",
            replyMarkup: Keyboards.InlineKeyboards.TagWithAdd)).MessageId;
    }

    public static async Task<int> TaggerWithActivate(ITelegramBotClient botClient, long chatId)
    {
        return (await botClient.SendTextMessageAsync(chatId, "Выберите опцию!",
            replyMarkup: Keyboards.InlineKeyboards.TagWithActivate)).MessageId;
    }

    public static async Task UserInUsersMenu(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(chatId, "Вы перешли в *меню пользователя*!",
            replyMarkup: Keyboards.ReplyKeyboards.SecondKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task UserInTacticsMenu(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(chatId, "Вы перешли в *меню тактик*!", 
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task<int> RandomizingTactic(ITelegramBotClient botClient, long chatId)
    {
        return (await botClient.SendTextMessageAsync(chatId,
            "Выберите карту для рандомного вывода коллов:\n\n*P.S Вам будут доступны только тактики, которые вы создали" +
            "и которые доступны вам. Не создавайте муссорных тактик, если не хотите, чтобы они вам попались* ",
            replyMarkup: Keyboards.InlineKeyboards.MapKeyboard, parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task EditRandomizingTactic(ITelegramBotClient botClient, long chatId, string data)
    {
        await botClient.SendTextMessageAsync(chatId,
            $"Выберите карту для рандомного вывода коллов:\n\nВы выбрали: *{data}* ✅", parseMode: ParseMode.Markdown);
    }
}