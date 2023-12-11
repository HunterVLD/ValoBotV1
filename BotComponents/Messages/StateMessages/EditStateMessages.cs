using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ValoBotV1.BotComponents.Messages.StateMessages;

public static class EditStateMessages
{
    public static async Task ChooseEditionOption(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(context.ChatId,
            "Выберите что будете редактировать:", replyMarkup: Keyboards.InlineKeyboards.EditFilterKeyboard)).MessageId;
    }
    
    public static async Task EditChooseEditionOption(ITelegramBotClient botClient, Context context, string data)
    {
        await botClient.EditMessageTextAsync(context.ChatId, context.LastBotMessageId,
            $"Выберите что будете редактировать:\nВы выбрали * {data} *✅",
            parseMode: ParseMode.Markdown);
    }
    
    public static async Task EnterNewTacticName(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, "Введите новое название:",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard);
    }

    public static async Task EnterNewTacticPolicy(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, "Выберите тип доступа:",
            replyMarkup: Keyboards.InlineKeyboards.TacticPolicy);
    }
    
    public static async Task EnterNewTacticAccessGroup(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, "Выберите действие:",
            replyMarkup: Keyboards.InlineKeyboards.EditPolicyAccessGroupVariants);
    }
    
    public static async Task EnterNewTacticMap(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(context.ChatId, 
            "Выберите новую карту:", replyMarkup: Keyboards.InlineKeyboards.MapKeyboard)).MessageId;
    }
    
    public static async Task EnterNewTacticSide(ITelegramBotClient botClient, Context context)
    {
        context.LastBotMessageId = (await botClient.SendTextMessageAsync(context.ChatId, 
            "Выберите новую cторону:", replyMarkup: Keyboards.InlineKeyboards.AtckOrDefKeyboard)).MessageId;
    }
    
    public static async Task EnterNewTacticForSite(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Введите новую тактику для выбраного сайта:",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard);
    }
    
    public static async Task EnterNewTacticDescription(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Введите новое Описание/пик:",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard);
    }
    
    public static async Task EnterNewTacticPhoto(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Введите новую ссылку для фото:",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard);
    }
    
    public static async Task PrivacyEditionComplete(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Доступ успешно изменен! ✅",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task EnterNamesToDelete(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Введите имена пользовател-ей(я), котор-ых(ого) хотите удалить:\n" +
            "* P.S Вводить пользователей имена пользователей нужно через пробел, запятую или точку," +
            "так же вводить имена пользователей без символа |@| *", 
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown);
    }
    
    public static async Task EnterNamesToAdd(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Введите имена пользовател-ей(я), котор-ых(ого) хотите добавить:\n" +
            "* P.S Вводить пользователей имена пользователей нужно через пробел, запятую или точку," +
            "так же вводить имена пользователей без символа |@| *", 
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown);
    }
    
    public static async Task UserDeletionComplete(ITelegramBotClient botClient, Context context, string user)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"Доступ пользователя * {user} * успешно удален! ✅", 
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task UserAddComplete(ITelegramBotClient botClient, Context context, string user)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"Доступ пользователя * {user} * успешно добавлен! ✅", 
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task NewTacticNameInDb(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"Новое название успешно записано в базу данных! ✅\n" +
            $"Новое название: * {context.data.ElementAt(3)} *",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup,
            parseMode: ParseMode.Markdown);
    }
    
    public static async Task NewTacticMapInDb(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Новая карта успешно записано в базу данных! ✅",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task NewTacticSideInDb(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Новая сторона успешно записано в базу данных! ✅",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task NewTacticSiteAbcInDb(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Новая тактика для сайта успешно записано в базу данных! ✅",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task NewTacticDescriptionInDb(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Новое описание/пик успешно записано в базу данных! ✅",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task NewTacticPhotoInDb(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Новая ссылка на фото успешно записано в базу данных! ✅",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
}