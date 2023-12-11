using System.Text.RegularExpressions;
using MongoDB.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using ValoBotV1.BotComponents;
using ValoBotV1.BotComponents.Messages;
using ValoBotV1.BotComponents.Messages.StateMessages;
using ValoBotV1.Validators;

namespace ValoBotV1.AllStateActions;

public class RandomizingTacticAction
{
    public static readonly Dictionary<Enum,
            Func<Context, Update, ITelegramBotClient, DataBase, Task>>
        RandomizeTactic = new() 
        {
            {Constants.RandomizingSteps.SelectionMap,
                async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                {
                    await botClient.DeleteMessageAsync(context.ChatId, context.LastBotMessageId);
                    
                    context.data.Add(update.CallbackQuery.From.Username); //record name
                    context.data.Add(update.CallbackQuery.Data); //record map
                    context.data.Add("ForSidePlace"); //add cringe place for side
                    
                    await ReplyButtonsMessages.EditRandomizingTactic(botClient, context.ChatId, update.CallbackQuery.Data);
                    await CreateStateMessages.ChooseSide(botClient, context);

                    context.State = Constants.RandomizingSteps.SelectionSide;
                }},
            {Constants.RandomizingSteps.SelectionSide,
                async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                {
                    await CreateStateMessages.EditChooseSide(botClient, context, update.CallbackQuery.Data);
                    
                    context.data[2] = update.CallbackQuery.Data; //record side

                    if (!await DBValidator.IsUserHaveCreatedAndAccessTacticByMap(update.CallbackQuery.From.Id,
                            context.data.ElementAt(1), context.data.ElementAt(2)))
                    {
                        await RandomizingStateMessages.UserDontHaveTacticByParams(botClient, context);
                        
                        context.State = Constants.RandomizingSteps.Complete;
                        return;
                    }

                    string pastingData = "";

                    pastingData = await dataBase.GetMyTacticsByMap(context.data);

                    await SearchStateMessages.PasteSearchedData(botClient, context, pastingData, "cancelWithRand");
                    
                    context.State = Constants.RandomizingSteps.Randomizing;
                }},
            {Constants.RandomizingSteps.Randomizing,
                async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                {
                    if (update.Message.Text == "Продолжить рандом 🔄")
                    {
                        string pastingData = "";

                        pastingData = await dataBase.GetMyTacticsByMap(context.data);

                        await SearchStateMessages.PasteSearchedData(botClient, context, pastingData, "cancelWithRand");
                    }

                    if (update.Message.Text == "Сменить сторону 🛡🗡")
                    {
                        /*context.data[2] = context.data.ElementAt(2) == "Attack" ? 
                            "Defense" : "Attack";*/
                        
                        /*if (!await DBValidator.IsUserHaveCreatedAndAccessTacticByMap(update.Message.From.Id,
                                context.data.ElementAt(1), context.data.ElementAt(2)))
                        {
                            await RandomizingStateMessages.UserDontHaveTacticByParams(botClient, context);

                            context.State = Constants.RandomizingSteps.Complete;
                            return;
                        }*/

                        context.State = Constants.RandomizingSteps.SelectionSide;
                        
                        await CreateStateMessages.ChooseSide(botClient, context);
                    }
                    
                }},
        };
}