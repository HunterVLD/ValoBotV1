using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ValoBotV1.BotComponents.Messages.StateMessages;

public static class CreateStateMessages
{
    public static async Task ChoosePrivacy(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(context.ChatId,
                "Выберите доступность вашей тактики:\n\n" +
                "*P.S Если вы выберете Public, то ваша тактика будет доступна другим пользователям.*",
                replyMarkup: Keyboards.InlineKeyboards.TacticPolicy, parseMode: ParseMode.Markdown)).MessageId;
    }

    public static async Task EditChoosePrivacy(ITelegramBotClient botClient, Context context, string data)
    {
        await botClient.EditMessageTextAsync(context.ChatId, context.LastBotMessageId,
            $"Выберите доступность вашей тактики:\nВы выбрали * {data} *✅",
            parseMode: ParseMode.Markdown);
    }
    
    public static async Task ChooseMap(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(context.ChatId,
            "Выберите карту:", replyMarkup: Keyboards.InlineKeyboards.MapKeyboard)).MessageId;
    }
    
    public static async Task EditChooseMap(ITelegramBotClient botClient, Context context, string data)
    {
        await botClient.EditMessageTextAsync(context.ChatId, context.LastBotMessageId,
            $"Выберите карту:\nВы выбрали * {data} *✅", parseMode: ParseMode.Markdown);
    }
    
    public static async Task ChooseUserForAccess(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(context.ChatId,
            "Выберите пользователей, которым будет доступна ваша приватная тактика:\n\n" +
            "*P.S Введите Ники Телеграм пользователей через пробел или запятую, " +
            "если не хотите добавлять пользователей, то введите символ - !*",
            parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task EditChooseUserForAccess(ITelegramBotClient botClient, Context context)
    {
        await botClient.EditMessageTextAsync(context.ChatId, context.LastBotMessageId,
            "Выберите пользователей, которым будет доступна ваша приватная тактика:\n" +
            $"Вы выбрали * {context.data.ElementAt(3)} *✅", parseMode: ParseMode.Markdown);
    }
    
    public static async Task ChooseSide(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(context.ChatId,
            "Выберите сторону: ", replyMarkup: Keyboards.InlineKeyboards.AtckOrDefKeyboard)).MessageId;
    }
    
    public static async Task EditChooseSide(ITelegramBotClient botClient, Context context, string data)
    {
        await botClient.EditMessageTextAsync(context.ChatId, context.LastBotMessageId,
            $"Выберите сторону:\nВы выбрали * {data} *✅", parseMode: ParseMode.Markdown);
    }
    
    public static async Task CreateASiteTactic(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(
            context.ChatId,
            "Напишите тактику для сайта * А: *",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task CreateBSiteTactic(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(
            context.ChatId,
            "Напишите тактику для сайта * B: *",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task CreateCSiteTactic(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(
            context.ChatId,
            "Напишите тактику для сайта * C: *",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task CreateDescriptionOrPeak(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(
            context.ChatId,
            "Введите * описание тактики(пик героев возможно): *",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task AddPhotoToTactic(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(
            context.ChatId,
            "Введите ссылку на возможное * фото: *",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown)).MessageId;
    }
    
    public static async Task TacticCreationComplete(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(
            context.ChatId,
            "* Вы успешно завершили создание тактики! * ✅",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown )).MessageId;
    }
    
    //todo ADD CHECK FOR USERS WARNING
}