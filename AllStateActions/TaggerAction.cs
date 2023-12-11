using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using ValoBotV1.BotComponents;
using ValoBotV1.BotComponents.Messages;
using ValoBotV1.BotComponents.Messages.StateMessages;
using ValoBotV1.Validators;

namespace ValoBotV1.AllStateActions;

public static class TaggerAction
{
    public static readonly Dictionary<Enum,
            Func<Context, Update, ITelegramBotClient, DataBase, Task>>
        Tagger = new() 
        {
            {Constants.TaggerSteps.Selection,
                async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                {
                    await botClient.DeleteMessageAsync(context.ChatId, context.LastBotMessageId);
                    
                    context.State = (Constants.TaggerSteps)Enum.Parse(typeof(Constants.TaggerSteps),
                        update.CallbackQuery.Data);

                    switch (context.State)
                    {
                        case Constants.TaggerSteps.AddTageMessage:
                            await TaggerActionMessages.EnterTaggMessage(botClient, context);
                            break;
                        case Constants.TaggerSteps.SendTageMessage:
                            string pastingData = await dataBase.GetTagMessage(update.CallbackQuery.From.Id);;

                            await TaggerActionMessages.PasteTag(botClient, context, pastingData);
                            await TaggerActionMessages.PasteTag(botClient, context, pastingData);
                            await TaggerActionMessages.PasteTag(botClient, context, pastingData);
                            await TaggerActionMessages.PasteTag(botClient, context, pastingData);
                            
                            context.State = Constants.TaggerSteps.Complete;
                            break;
                        default:
                            Console.WriteLine("TaggErrorSelection");
                            break;
                    }
                }},
            {Constants.TaggerSteps.AddTageMessage,
                async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                {
                    if (update.Message.Text.Length >= 224)
                    {
                        await WarningMessages.ALotOfTagSymbols(botClient, context.ChatId);
                        
                        return;
                    }

                    await dataBase.SetAndChangeUserTagMessage(update.Message.From.Id, 
                        update.Message.Text);

                    await TaggerActionMessages.SetTagComplete(botClient, context);
                    
                    context.State = Constants.TaggerSteps.Complete;
                }},
        };
}