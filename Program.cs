using MongoDB.Entities;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ValoBotV1.AllStateActions;
using ValoBotV1.BotComponents;
using ValoBotV1.BotComponents.Messages;
using ValoBotV1.Storage;
using ValoBotV1.Validators;
using File = Telegram.Bot.Types.File;

namespace ValoBotV1
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryStorage storage = new();
            
            TelegramBot bot = new(storage);
            
            bot.Start();

            Console.ReadLine();
            //todo cancel connection
        }
    }
    
    class TelegramBot
    {
        private readonly TelegramBotClient _botClient;

        private readonly CancellationTokenSource _cts;
        private readonly ReceiverOptions _receiverOptions;
       

        private readonly DataBase _dataBase;
        private readonly IStorage _storage;

        public delegate Task HandleStateCaller(Update update, long chatId);
        public static HandleStateCaller callState;

        private readonly Dictionary<string, Func<IStorage, ITelegramBotClient, Message,
            long, int, Task>> _messageSwitcher = new()
        {
            {"/start",
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId) =>
                {
                    await CommandMessages.StartCommand(botClient, chatId);
                }},
            {"/help", //todo in WEB
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    await botClient.SendTextMessageAsync(chatId, Constants.CommandInformation[0], 
                        replyMarkup: Keyboards.ReplyKeyboards.MainKeyboardMarkup); 
                }},
            {"/info", 
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    await CommandMessages.InfoCommand(botClient, chatId);
                }},
            {"Создание тактики 🧠",
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    if (message.Chat.Type == ChatType.Group)
                    {
                        await WarningMessages.UserCanNotUseThisInGroup(botClient, chatId);
                        
                        return;
                    }

                    botMessageId = await ReplyButtonsMessages.CreateTactic(botClient, chatId);
                    
                    storage.AddContext(new Context(Constants.ContextTypes.TacticCreationContext, botMessageId,
                        Constants.CreationSteps.TacticName, chatId));
                }},
            {"Тактики 🔍",
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    botMessageId = await ReplyButtonsMessages.SearchTactic(botClient, chatId);

                    storage.AddContext(new Context(Constants.ContextTypes.TacticSearchContext, botMessageId,
                        Constants.SearchSteps.Selection, chatId));
                }},
            {"Изменить тактику 📝", 
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    if (message.Chat.Type == ChatType.Group)
                    {
                        await WarningMessages.UserCanNotUseThisInGroup(botClient, chatId);
                        
                        return;
                    }

                    botMessageId = await ReplyButtonsMessages.EditTactic(botClient, chatId);

                    storage.AddContext(new Context(Constants.ContextTypes.TacticEditContext, botMessageId,
                        Constants.EditSteps.EnterName, chatId));
                }},
            {"Удалить тактику 📛", 
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    if (message.Chat.Type == ChatType.Group)
                    {
                        await WarningMessages.UserCanNotUseThisInGroup(botClient, chatId);
                        
                        return;
                    }

                    botMessageId = await ReplyButtonsMessages.DeleteTactic(botClient, chatId);
                    
                    storage.AddContext(new Context(Constants.ContextTypes.TacticDeleteContext, botMessageId,
                        Constants.DeleteSteps.EnterOneMoreNames, chatId));
                }},
            {"Посмотреть KDA 🦾", //todo learn how to parse data from any site
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    //todo write hard code with parsing and new state machine for actions
                }},
            {"Групповой тэг 🔔", //todo this shit
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    if (message.Chat.Type != ChatType.Group)
                    {
                        await WarningMessages.UserCanUseOnlyThisInGroup(botClient, chatId);
                        
                        return;
                    }

                    if (await DBValidator.IsUserHaveTagMessage(message.From.Id))
                    {
                        botMessageId = await ReplyButtonsMessages.TaggerWithActivate(botClient, chatId);
                    }
                    else
                    {
                        botMessageId = await ReplyButtonsMessages.TaggerWithAddingTag(botClient, chatId);
                    }
                    
                    storage.AddContext(new Context(Constants.ContextTypes.TaggerContext, botMessageId,
                        Constants.TaggerSteps.Selection, chatId));
                }},
            {"Рандомайзер коллов 🔴",
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    botMessageId = await ReplyButtonsMessages.RandomizingTactic(botClient, chatId);
                    
                    storage.AddContext(new Context(Constants.ContextTypes.TacticRandomizerContext, botMessageId,
                        Constants.RandomizingSteps.SelectionMap, chatId));
                }},
            {"Отменить задачу ❌",
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    storage.DeleteContext(chatId);
                    await ReplyButtonsMessages.CancelAction(botClient, chatId);
                }},
            {"Меню пользоватлея 🔻🔻",
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    await ReplyButtonsMessages.UserInUsersMenu(botClient, chatId);
                }},
            {"Вернуться в меню для тактик  🔺🔺",
                async (IStorage storage, ITelegramBotClient botClient, Message message, long chatId, int botMessageId)  =>
                {
                    await ReplyButtonsMessages.UserInTacticsMenu(botClient, chatId);
                }},
        };

        public TelegramBot(IStorage storage)
        {
            _botClient = new(Constants.Token[0]);
            _cts = new();
            _receiverOptions = new()
            {
                AllowedUpdates = new UpdateType[] { UpdateType.Message, UpdateType.CallbackQuery},
                ThrowPendingUpdates = true
            };
            _dataBase = new();
            _storage = storage;

            callState = new(HandleStateMachine);
        }
        public async void Start()
        {
            _botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: _receiverOptions,
                cancellationToken: _cts.Token
            );
            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");

            await _dataBase.ExecuteDataBase();
            Console.WriteLine("DataBase Executed!");
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (UpdatesValidator.IsUpdateNullReferenced(update))
                return;
            if (UpdatesValidator.IsUpdateDontHaveUserName(update)) {
                await WarningMessages.UserDontHaveName(_botClient, update);
                
                return;
            }

            string userName = UpdatesValidator.IsTypeUpdateMessage(update) ?
                update.Message.From.Username : update.CallbackQuery.From.Username;
            long userId = UpdatesValidator.IsTypeUpdateMessage(update) ? 
                update.Message.From.Id : update.CallbackQuery.From.Id;
            long chatId = UpdatesValidator.IsTypeUpdateMessage(update) ? 
                update.Message.Chat.Id : update.CallbackQuery.Message.Chat.Id;
            
            if(UpdatesValidator.IsOperationCancelled(update))
                await HandleMessage(_botClient, update, chatId);
            else if (!await DBValidator.IsUserAlreadyCreated(userId))
            {
                await _dataBase.CreateUser(userName ?? throw new InvalidOperationException(), userId); //todo check exception
                await HandleMessage(_botClient, update, chatId);
            }
            else if(await DBValidator.IsUserChangeName(userName, userId))
                await _dataBase.ChangeUserName(userName, userId);
            else if (_storage.ContainsContext(chatId))
                await HandleStateMachine(update, chatId);
            else if (update.Message != null)
                await HandleMessage(_botClient, update, chatId);
        }

        async Task HandleStateMachine(Update update, long chatId) //todo Rewrite this a lot of cases mb
        {
            Context context = _storage.GetContext(chatId);
            
            if(UpdatesValidator.IsMessageForCallBackQueryState(context, update))
                return;

            switch (context.Type)
            {
                case Constants.ContextTypes.TacticCreationContext:
                    await CreateTacticAction.CreateTactic[context.State](context, update, _botClient, _dataBase);
                    if (context.State.Equals(Constants.CreationSteps.Complete)) {
                        _storage.DeleteContext(context.ChatId);
                    }
                    return;
                case Constants.ContextTypes.TacticSearchContext:
                    await SearchTacticAction.SearchTactic[context.State](context, update, _botClient, _dataBase);
                    if (context.State.Equals(Constants.SearchSteps.Complete))
                    {
                        _storage.DeleteContext(context.ChatId);
                    }
                    return;
                case Constants.ContextTypes.TacticEditContext:
                    await EditTacticAction.TacticEditActions[context.State](context, update, _botClient, _dataBase);
                    if (context.State.Equals(Constants.EditSteps.Complete))
                    {
                        _storage.DeleteContext(context.ChatId);
                    }
                    return;
                case Constants.ContextTypes.TacticDeleteContext:
                    await DeleteTacticAction.DeleteTactic[context.State](context, update, _botClient, _dataBase);
                    if (context.State.Equals(Constants.DeleteSteps.Complete))
                    {
                        _storage.DeleteContext(context.ChatId);
                    }
                    break;
                case Constants.ContextTypes.TacticRandomizerContext:
                 await RandomizingTacticAction.RandomizeTactic[context.State](context, update, _botClient, _dataBase);
                    if (context.State.Equals(Constants.RandomizingSteps.Complete))
                    {
                        _storage.DeleteContext(context.ChatId);
                    }
                    break;
                case Constants.ContextTypes.TaggerContext:
                    await TaggerAction.Tagger[context.State](context, update, _botClient, _dataBase);
                    if (context.State.Equals(Constants.TaggerSteps.Complete))
                    {
                        _storage.DeleteContext(context.ChatId);
                    }
                    break;
                default:
                    return;
                
            }
        }
        
        async Task HandleMessage(ITelegramBotClient botClient, Update update, long chatId)
        {
            var message = update.Message;
            int botMessageId = 0;

            if (_messageSwitcher.ContainsKey(message.Text))
                await _messageSwitcher[message.Text](_storage, _botClient, message, chatId, botMessageId);
        }
        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
    }
    
    
}
