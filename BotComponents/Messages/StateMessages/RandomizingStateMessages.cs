using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace ValoBotV1.BotComponents.Messages.StateMessages;

public static class RandomizingStateMessages
{
    public static async Task UserDontHaveTacticByParams(ITelegramBotClient botClient, Context context)
    {
        await botClient.SendTextMessageAsync(context.ChatId,
            "У вас нету тактик под заданную карту и сторону! 🟥\n*Проверьте ваши тактики.*",
            replyMarkup: Keyboards.ReplyKeyboards.SecondKeyboardMarkup, parseMode: ParseMode.Markdown);
    }
}