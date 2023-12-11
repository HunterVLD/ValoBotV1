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
    public static class CreateTacticAction
    {
        public static readonly Dictionary<Enum,
                Func<Context, Update, ITelegramBotClient, DataBase, Task>>
            CreateTactic = new()
            {
                {
                    Constants.CreationSteps.TacticName,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        await botClient.DeleteMessageAsync(context.ChatId, context.LastBotMessageId);
                        
                        context.data.Add(update.Message.From.Username); // ad name to datalist | check down (1)
                        context.data.Add(Regex.Replace(update.Message.Text,
                            "[@, /`~&*+:^<>%!?\\.\";'\\\\]",
                            string.Empty)); // add to dataList after user enter paste his data (2)
                        
                        await ReplyButtonsMessages.EditCreateTactic(botClient, context.ChatId,
                            context.data.ElementAt(1));

                        if (await DBValidator.IsTacticNameAlreadyExist(context.data.ElementAt(1)))
                        {
                            await WarningMessages.TacticNameAlreadyExist(botClient, context);
                            return;
                        } // check tactic name exist in data base

                        await CreateStateMessages.ChoosePrivacy(botClient, context);

                        context.State = Constants.CreationSteps.ChooseTacticPolicy; // Change State for next
                    }
                },
                {
                    Constants.CreationSteps.ChooseTacticPolicy,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(update.CallbackQuery.Data); // add tactic policy in db (3)

                        await CreateStateMessages.EditChoosePrivacy(botClient, context, update.CallbackQuery.Data);

                        if (update.CallbackQuery.Data == "Public")
                        {
                            context.data.Add("Все пользователи"); //add group access data

                            await CreateStateMessages.ChooseMap(botClient, context);

                            context.State = Constants.CreationSteps.Map;
                        }
                        else if (update.CallbackQuery.Data == "Private")
                        {
                            await CreateStateMessages.ChooseUserForAccess(botClient, context);

                            context.State = Constants.CreationSteps.ChooseTacticGroup;
                        }
                    }
                },
                {
                    Constants.CreationSteps.ChooseTacticGroup,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        string[] accessibleUsers = Regex.Split(update.Message.Text, @"[^0-9a-zA-Z_]+");

                        foreach (var item in accessibleUsers)
                        {
                            Console.WriteLine(item);
                        }

                        string users = "";
                        foreach (var user in accessibleUsers)
                        {
                            if (await DBValidator.IsUserExist(user))
                            {
                                users += user + " ";
                            }
                            else
                            {
                                await WarningMessages.UserDontExist(botClient, context, user);
                            }
                        }

                        if (users == "" || users == " ") // add tactic policy group array in db (4)
                        {
                            context.data.Add("Отсутсвует");
                        }
                        else 
                        {
                            context.data.Add(users);
                        }
                         
                        
                        await CreateStateMessages.EditChooseUserForAccess(botClient, context);

                        await CreateStateMessages.ChooseMap(botClient, context);

                        context.State = Constants.CreationSteps.Map;
                    }
                },
                {
                    Constants.CreationSteps.Map,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        await CreateStateMessages.EditChooseMap(botClient, context, update.CallbackQuery.Data);

                        context.data.Add(update.CallbackQuery.Data);

                        await CreateStateMessages.ChooseSide(botClient, context);

                        context.State = Constants.CreationSteps.AtckDef;
                    }
                },
                {
                    Constants.CreationSteps.AtckDef,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        await CreateStateMessages.EditChooseSide(botClient, context, update.CallbackQuery.Data);

                        context.data.Add(update.CallbackQuery.Data);

                        await CreateStateMessages.CreateASiteTactic(botClient, context);

                        context.State = Constants.CreationSteps.ASiteTactic;
                    }
                },
                {
                    Constants.CreationSteps.ASiteTactic,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(update.Message.Text);

                        await CreateStateMessages.CreateBSiteTactic(botClient, context);

                        context.State = Constants.CreationSteps.BSiteTactic;
                    }
                },
                {
                    Constants.CreationSteps.BSiteTactic,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(update.Message.Text);

                        await CreateStateMessages.CreateCSiteTactic(botClient, context);

                        context.State = Constants.CreationSteps.CSiteTactic;
                    }
                },
                {
                    Constants.CreationSteps.CSiteTactic,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(update.Message.Text);

                        await CreateStateMessages.CreateDescriptionOrPeak(botClient, context);

                        context.State = Constants.CreationSteps.Description;
                    }
                },
                {
                    Constants.CreationSteps.Description,
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(update.Message.Text);

                        await CreateStateMessages.AddPhotoToTactic(botClient, context);

                        context.State = Constants.CreationSteps.PhotoLink;
                    }
                },
                {
                    Constants.CreationSteps.PhotoLink, 
                    async (Context context, Update update, ITelegramBotClient botClient, DataBase dataBase) =>
                    {
                        context.data.Add(update.Message.Text);

                        await dataBase.CheckDataAndSet(context.data);

                        await CreateStateMessages.TacticCreationComplete(botClient, context);

                        context.State = Constants.CreationSteps.Complete;
                    }
                }
            };
    }
}

