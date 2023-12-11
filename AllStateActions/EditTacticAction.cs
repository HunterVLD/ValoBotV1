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
    public static class EditTacticAction
    {
        public static readonly Dictionary<Enum,
                Func<Context, Update, ITelegramBotClient, DataBase, Task>>
            TacticEditActions = new()
            {
                {Constants.EditSteps.EnterName,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(update.Message.From.Username); // userName
                        context.data.Add(update.Message.Text); // tacticName

                        if (!await DBValidator.IsUserHaveOwnTactic(context.data.ElementAt(0), context.data.ElementAt(1)))
                        {
                            await WarningMessages.UserDontHavePermissionToEdit(botClient, context);

                            context.State = Constants.EditSteps.Complete;
                            return;
                        }

                        await EditStateMessages.ChooseEditionOption(botClient, context);

                        context.State = Constants.EditSteps.Selection;
                    }},
                {Constants.EditSteps.Selection,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.State = (Constants.EditSteps)Enum.Parse(typeof(Constants.EditSteps),
                            update.CallbackQuery.Data);

                        if (DBValidator.IsTacticPrivacyPublic(context.data.ElementAt(1)).Result &&
                            context.State.Equals(Constants.EditSteps.EditTacticAccessGroup))
                        {
                            await WarningMessages.CanNotEditAccessPublicGroup(botClient, context);

                            context.State = Constants.EditSteps.Complete;
                        } //check for public and current state

                        await EditStateMessages.EditChooseEditionOption(botClient, context, update.CallbackQuery.Data);

                        switch (context.State)
                        {
                            case Constants.EditSteps.EditTacticName:
                                await EditStateMessages.EnterNewTacticName(botClient, context);
                                break;
                            case Constants.EditSteps.EditTacticPolicy:
                                await EditStateMessages.EnterNewTacticPolicy(botClient, context);
                                break;
                            case Constants.EditSteps.EditTacticAccessGroup:
                                await EditStateMessages.EnterNewTacticAccessGroup(botClient, context);
                                break;
                            case Constants.EditSteps.EditTacticMap:
                                await EditStateMessages.EnterNewTacticMap(botClient, context);
                                break;
                            case Constants.EditSteps.EditTacticAttckDef:
                                await EditStateMessages.EnterNewTacticSide(botClient, context);
                                break;
                            case Constants.EditSteps.EditTacticSiteA or Constants.EditSteps.EditTacticSiteB
                                or Constants.EditSteps.EditTacticSiteC:
                                await EditStateMessages.EnterNewTacticForSite(botClient, context);
                                break;
                            case Constants.EditSteps.EditTacticDescription:
                                await EditStateMessages.EnterNewTacticDescription(botClient, context);
                                break;
                            case Constants.EditSteps.EditTacticPhoto:
                                await EditStateMessages.EnterNewTacticPhoto(botClient, context);
                                break;
                            default:
                                Console.WriteLine("Oshibka");
                                break;
                        }
                            
                    }},
                {Constants.EditSteps.EditTacticPolicy,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.CallbackQuery.Data); // data to change

                        await EditStateMessages.PrivacyEditionComplete(botClient, context);

                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));
                        
                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticAccessGroup,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.CallbackQuery.Data); // one more filter
                        
                        context.State = (Constants.EditSteps)Enum.Parse(typeof(Constants.EditSteps),
                            update.CallbackQuery.Data);
                        
                        if (context.State.Equals(Constants.EditSteps.DeleteUsers))
                            await EditStateMessages.EnterNamesToDelete(botClient, context);
                        else if (context.State.Equals(Constants.EditSteps.AddUsers))
                            await EditStateMessages.EnterNamesToAdd(botClient, context);
                        
                    }},
                {Constants.EditSteps.DeleteUsers,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        string[] userToDeleteNames = Regex.Split(update.Message.Text, @"[^0-9a-zA-Z_]+");

                        foreach (var user in userToDeleteNames)
                        {
                            if (!DBValidator.IsUserExist(user).Result)
                            {
                                await WarningMessages.CanNotDeleteNonExistingUser(botClient, context, user);

                                continue;
                            }
                            if (!DBValidator.IsUserHaveAccessToTactic(user, context.data.ElementAt(1)).Result)
                            {
                                await WarningMessages.CanNotDeleteUserWithNoAccess(botClient, context, user);

                                continue;
                            }

                            await dataBase.DeleteTacticUserAccess(context.data.ElementAt(1), user);

                            await EditStateMessages.UserDeletionComplete(botClient, context, user);
                        }
                        
                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.AddUsers,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        string[] userToDeleteNames = Regex.Split(update.Message.Text, @"[^0-9a-zA-Z_]+");

                        foreach (var user in userToDeleteNames)
                        {
                            if (!DBValidator.IsUserExist(user).Result)
                            {
                                await WarningMessages.CanNotAddNonExistingUser(botClient, context, user);

                                continue;
                            }
                            if (DBValidator.IsUserHaveAccessToTactic(user, context.data.ElementAt(1)).Result)
                            {
                                await WarningMessages.CanNotAddUserWithAccess(botClient, context, user);

                                continue;
                            }

                            await dataBase.AddTacticUserAccess(context.data.ElementAt(1), user);

                            await EditStateMessages.UserAddComplete(botClient, context, user);
                        }
                        
                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticName,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        if (DBValidator.IsTacticNameAlreadyExist(update.Message.Text).Result)
                        {
                            await WarningMessages.TacticNameAlreadyExist(botClient, context);

                            return;
                        }
                        
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(Regex.Replace(update.Message.Text, //data to change
                            "[@, /`~&*+:^<>%\\.\";'\\\\]",
                            string.Empty));

                        await EditStateMessages.NewTacticNameInDb(botClient, context);

                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));
                        
                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticMap,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.CallbackQuery.Data);

                        await EditStateMessages.NewTacticMapInDb(botClient, context);
                        
                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));

                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticAttckDef,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.CallbackQuery.Data);

                        await EditStateMessages.NewTacticSideInDb(botClient, context);
                        
                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));

                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticSiteA,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.Message.Text);

                        await EditStateMessages.NewTacticSiteAbcInDb(botClient, context);
                        
                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));

                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticSiteB,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.Message.Text);

                        await EditStateMessages.NewTacticSiteAbcInDb(botClient, context);
                        
                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));

                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticSiteC,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.Message.Text);
                        
                        await EditStateMessages.NewTacticSiteAbcInDb(botClient, context);

                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));

                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticDescription,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.Message.Text);

                        await EditStateMessages.NewTacticDescriptionInDb(botClient, context);
                        
                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));

                        context.State = Constants.EditSteps.Complete;
                    }},
                {Constants.EditSteps.EditTacticPhoto,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(context.State.ToString()); // filterType
                        context.data.Add(update.Message.Text);

                        await EditStateMessages.NewTacticPhotoInDb(botClient, context);
                        
                        await dataBase.CheckFilterAndUpdate(context.data.ElementAt(1),
                            context.data.ElementAt(0), context.data.ElementAt(2),
                            context.data.ElementAt(3));

                        context.State = Constants.EditSteps.Complete;
                    }},
            };
        
    }   
}