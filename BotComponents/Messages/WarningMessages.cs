using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ValoBotV1.Validators;

namespace ValoBotV1.BotComponents.Messages;

public static class WarningMessages
{
    public static async Task UserDontHaveName(ITelegramBotClient botClient, Update update)
    {
        long chatId = UpdatesValidator.IsTypeUpdateMessage(update) ? 
            update.Message.Chat.Id : update.CallbackQuery.Message.Chat.Id;

        await botClient.SendTextMessageAsync(chatId,
            "*У вас отсутствует имя пользователя в телеграме!* 🟥\n\nПопробуйте в настройках пользователя задать никнейм.",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task UserDontExist(ITelegramBotClient botClient, Context context, string userName)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"Пользователя *{userName}* - не существует в базе данных! 🟨\n\n*Так как он не пользовался ботом*",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown);
    }
    
    public static async Task UserCanNotUseThisInGroup(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(chatId,
            "Вы не можете использовать эту функцию в группе! Попробуйте приватный чат.",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task ALotOfTagSymbols(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(chatId,
            "Вы не можете в тег ввести больше *224* символов 🟥\n\nПопробуйте еще раз:",
            replyMarkup: Keyboards.ReplyKeyboards.SecondKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task UserCanUseOnlyThisInGroup(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendTextMessageAsync(chatId,
            "Вы не можете использовать эту функцию не в группе! Попробуйте в группе.",
            replyMarkup: Keyboards.ReplyKeyboards.SecondKeyboardMarkup);
    }
    public static async Task UserDontHavePermissionToEdit(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "Вы не можете редактировать не свою тактику! 🟥",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup);
    }
    
    public static async Task TacticNameAlreadyExist(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, 
            "Данное название занято! 🟥\n\n*Попробуйте снова или отмените задачу:*",
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown);
    }
    
    public static async Task TacticNameDontExist(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, 
            "Данного имени не существует в базе данных! 🟥\n\n*Попробуйте снова или отмените задачу:*", 
            replyMarkup: Keyboards.ReplyKeyboards.CancellationCreateKeyboard, parseMode: ParseMode.Markdown);
    }
    
    public static async Task CanNotEditAccessPublicGroup(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId, 
            "Вы не можете менять людей, которым будет доступна эта тактика, поскольку она * Public * ! 🟥",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task CanNotDeleteNonExistingUser(ITelegramBotClient botClient, Context context, string user)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"Вы не можете удалить не существующего пользователя * {user} * ! 🟨",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task CanNotAddNonExistingUser(ITelegramBotClient botClient, Context context, string user)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"Вы не можете добавить не существующего пользователя * {user} * ! 🟨",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task CanNotDeleteUserWithNoAccess(ITelegramBotClient botClient, Context context, string user)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"* {user} * изначально не имеет доступа к вашей тактике! 🟨",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task CanNotAddUserWithAccess(ITelegramBotClient botClient, Context context, string user)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"* {user} * изначально имеет доступ к вашей тактике! 🟨",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    public static async Task CanNotDeleteNotOwnTactic(ITelegramBotClient botClient, Context context, string name)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"Вы не можете удалить не свою тактику * {name} * ! 🟥",
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
    
    
    
}