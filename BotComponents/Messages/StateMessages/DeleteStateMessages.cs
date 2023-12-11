using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ValoBotV1.BotComponents.Messages.StateMessages;

public static class DeleteStateMessages
{
    public static async Task TacticDelitionCompleted(ITelegramBotClient botClient, Context context, string name)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            $"Ваша тактика: * {name}* \n Успешно удалена из базы данных! ✅", 
            replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
}