using System.Text.RegularExpressions;
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
    public static class DeleteTacticAction
    {
        public static readonly Dictionary<Enum,
                Func<Context, Update, ITelegramBotClient, DataBase, Task>>
            DeleteTactic = new() 
            {
                {Constants.DeleteSteps.EnterOneMoreNames,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(update.Message.From.Username);
                        
                        string[] tacticNames = Regex.Split(update.Message.Text, @"[^0-9a-zA-Z-_#$№А-Яа-я]+");

                        context.data.Add(update.Message.From.Username); // record username for checking

                        foreach (var name in tacticNames)
                        {
                            if (!await DBValidator.IsUserHaveOwnTactic(context.data.ElementAt(0), name))
                            {
                                await WarningMessages.CanNotDeleteNotOwnTactic(botClient, context, name);

                                continue;
                            }
                            
                            await dataBase.DeleteItem(name);

                            await DeleteStateMessages.TacticDelitionCompleted(botClient, context, name);

                        }
                        
                        context.State = Constants.DeleteSteps.Complete;
                    }},
            };
    }
}

