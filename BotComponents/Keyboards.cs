using Telegram.Bot.Types.ReplyMarkups;

namespace ValoBotV1.BotComponents
{
    public static class Keyboards
    {
        public static class ReplyKeyboards
        {
            public static readonly ReplyKeyboardMarkup MainKeyboardMarkup = new(new[]
            {
                new KeyboardButton[] { "Тактики 🔍", "Создание тактики 🧠" },
                new KeyboardButton[] { "Удалить тактику 📛", "Изменить тактику 📝" },
                new KeyboardButton[] { "Меню пользоватлея 🔻🔻" },
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = false
            };

            public static readonly ReplyKeyboardMarkup SecondKeyboardMarkup = new(new[]
            {
                //todo Try Do Parser for this new KeyboardButton[] { "Посмотреть KDA 🦾", "Рандомайзер коллов 🔴" },
                new KeyboardButton[] { "Групповой тэг 🔔", "Рандомайзер коллов 🔴" },
                new KeyboardButton[] { "Вернуться в меню для тактик  🔺🔺" },
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = false
            };

            public static readonly ReplyKeyboardMarkup CancellationCreateKeyboard = new(new[]
            {
                new KeyboardButton[] { "Отменить задачу ❌", },
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
            
            public static readonly ReplyKeyboardMarkup CancellationWithRandKeyboard = new(new[]
            {
                new KeyboardButton[] { "Продолжить рандом 🔄", "Отменить задачу ❌"},
                new KeyboardButton[] { "Сменить сторону 🛡🗡"},
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }

        public static class InlineKeyboards
        {
            public static readonly InlineKeyboardMarkup MapKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.MapNames[0]),
                    InlineKeyboardButton.WithCallbackData(Constants.MapNames[1]),
                    InlineKeyboardButton.WithCallbackData(Constants.MapNames[2]),
                    InlineKeyboardButton.WithCallbackData(Constants.MapNames[3]),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.MapNames[4]),
                    InlineKeyboardButton.WithCallbackData(Constants.MapNames[5]),
                    InlineKeyboardButton.WithCallbackData(Constants.MapNames[6]),
                    InlineKeyboardButton.WithCallbackData(Constants.MapNames[7]),
                },
            });
            
            public static readonly InlineKeyboardMarkup AtckOrDefKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.AtckDef[0]),
                    InlineKeyboardButton.WithCallbackData(Constants.AtckDef[1]),
                }
            });
            
            public static readonly InlineKeyboardMarkup TacticPolicy = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.TacticPolicyTypes[0]),
                    InlineKeyboardButton.WithCallbackData(Constants.TacticPolicyTypes[1]),
                }
            });
            
            public static readonly InlineKeyboardMarkup SearchFilterKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForSearching[0], 
                        (Constants.SearchSteps.SearchByName).ToString()),
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForSearching[1], 
                        (Constants.SearchSteps.GetAll).ToString()),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForSearching[4], 
                        (Constants.SearchSteps.GetMyTactics).ToString())
                },
            });
            
            public static readonly InlineKeyboardMarkup EditFilterKeyboard = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[0], 
                        (Constants.EditSteps.EditTacticName).ToString()),
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[1], 
                        (Constants.EditSteps.EditTacticMap).ToString()),
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[2], 
                        (Constants.EditSteps.EditTacticAttckDef).ToString()),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[3], 
                        (Constants.EditSteps.EditTacticSiteA).ToString()),
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[4], 
                        (Constants.EditSteps.EditTacticSiteB).ToString()),
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[5], 
                        (Constants.EditSteps.EditTacticSiteC).ToString()),
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[6], 
                        (Constants.EditSteps.EditTacticDescription).ToString()),
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[7], 
                        (Constants.EditSteps.EditTacticPhoto).ToString()),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[8], 
                        (Constants.EditSteps.EditTacticPolicy).ToString()),
                    InlineKeyboardButton.WithCallbackData(Constants.FiltersForEdit[9], 
                        (Constants.EditSteps.EditTacticAccessGroup).ToString()),
                },
            });
            
            public static readonly InlineKeyboardMarkup EditPolicyAccessGroupVariants = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.FilterToEditPolicyAccessGroup[0], 
                        Constants.EditSteps.DeleteUsers.ToString()),
                },
                new[]
                {
                InlineKeyboardButton.WithCallbackData(Constants.FilterToEditPolicyAccessGroup[1],
                    Constants.EditSteps.AddUsers.ToString()),
                }
            });
            
            public static readonly InlineKeyboardMarkup TagWithAdd = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.TaggerFilter[0], 
                        Constants.TaggerSteps.AddTageMessage.ToString()),
                },
            });
            
            public static readonly InlineKeyboardMarkup TagWithActivate = new(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.TaggerFilter[1], 
                        Constants.TaggerSteps.AddTageMessage.ToString()),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Constants.TaggerFilter[2], 
                        Constants.TaggerSteps.SendTageMessage.ToString()),
                },
            });
        }
        
    }
}

