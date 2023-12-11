using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ValoBotV1.BotComponents;
using ValoBotV1.BotComponents.Messages;
using ValoBotV1.BotComponents.Messages.StateMessages;
using ValoBotV1.Validators;
using Constants = ValoBotV1.BotComponents.Constants;

namespace ValoBotV1.AllStateActions
{
    public static class SearchTacticAction
    {
        public static readonly Dictionary<Enum,
                Func<Context, Update, ITelegramBotClient, DataBase, Task>>
            SearchTactic = new() 
            {
                {Constants.SearchSteps.Selection,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        await botClient.DeleteMessageAsync(context.ChatId, context.LastBotMessageId);
                        await botClient.SendChatActionAsync(context.ChatId, ChatAction.Typing);

                        context.data.Add(update.CallbackQuery.From.Username); //username to check policy 0
                        context.data.Add(update.CallbackQuery.Data); //filterType 1

                        context.State = (Constants.SearchSteps)Enum.Parse(typeof(Constants.SearchSteps),
                            update.CallbackQuery.Data);

                        string pastingData = "";
                        switch (context.State)
                        {
                            case Constants.SearchSteps.GetAll:
                                pastingData = await dataBase.GetCollectionItemByFilter(context.data);
                                await SearchStateMessages.PasteSearchedData(botClient, context, pastingData);
                                
                                context.State = Constants.SearchSteps.Complete;
                                break;
                            case Constants.SearchSteps.GetMyTactics:
                                pastingData = await dataBase.GetCollectionItemByFilter(context.data);
                                await SearchStateMessages.PasteSearchedData(botClient, context, pastingData);
                                
                                context.State = Constants.SearchSteps.Complete;
                                break;
                            default:
                                if (context.State.Equals(Constants.SearchSteps.SearchByName))
                                    await SearchStateMessages.EnterTacticName(botClient, context);
                                break;
                        }
                    }},
                {Constants.SearchSteps.SearchByName,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        string pastingData = "";
                        if (!await DBValidator.IsTacticNameAlreadyExist(update.Message.Text))
                        {
                            await WarningMessages.TacticNameDontExist(botClient, context);
                            return;
                        }
                        context.data.Add(update.Message.Text); // 2
                        
                        pastingData = await dataBase.GetCollectionItemByFilter(context.data);

                        await SearchStateMessages.PasteSearchedData(botClient, context, pastingData);
                        
                        context.State = Constants.SearchSteps.Complete;
                    }},
            };
    }
}