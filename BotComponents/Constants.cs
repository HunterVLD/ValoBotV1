namespace ValoBotV1.BotComponents
{
    public static class Constants
    {
        public static readonly string[] Token = new string[1]
        {
            "6090241740:AAE8CotSA26ZcVxMwkXrJVob43e77oCSy9M"
        };
        
        public static readonly string[] MapNames = new String[8] { 
            "Ascent", "Haven", "Split", "Bind", 
            "Icebox", "Pearl" ,"Fracture", "Breeze" 
        };
        
        public static readonly string[] AtckDef = new string[2] { "Attack", "Defense" };

        public static readonly string[] TacticPolicyTypes = new string[2] { "Private", "Public" };

        public static readonly string[] FiltersForSearching = new string[5] {
            "По названию", "Все",
            "По номеру", "По карте", "Мои тактики"
        };
        
        public static readonly string[] FiltersForEdit = new string[10] {
            "Название", "Карту", "Сторону",
            "А-сайт", "В-сайт", "С-сайт", "Пик", "Фото",
            "Тип Доступа", "Группу Доступа"
        };
        
        public static readonly string[] FilterToEditPolicyAccessGroup = new string[2] { "Удалить пользователей", 
            "Добавить пользователей" };
        
        public static readonly string[] TaggerFilter = new string[3] { "Добавить тэг", 
            "Изменить тэг",  "Активировать"};

        public enum ContextTypes : byte
        {
            TacticCreationContext,
            TacticSearchContext,
            TacticEditContext,
            TacticDeleteContext,
            TacticRandomizerContext,
            TaggerContext
        }
        
        public enum CreationSteps : byte
        {
            TacticName,
            ChooseTacticPolicy,
            ChooseTacticGroup, //optional
            Map,
            AtckDef,
            ASiteTactic,
            BSiteTactic,
            CSiteTactic,
            Description,
            PhotoLink,
            Complete
        }

        public enum SearchSteps : byte
        {
            Selection,
            SearchByName,
            
            GetAll,        //all 3 items is optional
            GetMyTactics,
            
            Complete
        }
        
        public enum EditSteps : byte
        {
            EnterName,
            Selection,
            
            EditTacticName,
            EditTacticPolicy,
            
            EditTacticAccessGroup,
            AddUsers,
            DeleteUsers,

            EditTacticMap,
            EditTacticAttckDef,
            EditTacticSiteA,
            EditTacticSiteB,
            EditTacticSiteC,
            EditTacticDescription,
            EditTacticPhoto,
            
            Complete
        }
        
        public enum DeleteSteps : byte
        {
            EnterOneMoreNames,
            Complete
        }
        
        public enum RandomizingSteps : byte
        {
            SelectionMap,
            SelectionSide,
            Randomizing,
            Complete
        }
        
        public enum TaggerSteps : byte
        {
            Selection,
            SendTageMessage,
            AddTageMessage,
            Complete
        }
        
        public static readonly string[] CommandInformation = new String[2]
        {
            "1) Кнопка: | Создание тактики 🧠 |\n" +
            "-С помощью этой кнопки вы можете записать свою тактику в базу данных для дальнейшего ее просмотра" +
            "и использование. Вам всего лишь нужно будет заполнить данные, которые от вас требует бот.\n" +
            "2) Кнопка: | Тактики 🔍 |\n" +
            "-C помощью этой кнопки вы можете найти тактику, которую создали для того, чтобы просмотреть её детальнее." +
            "Так же вы можете использовть множество разных фильтров для поиска тактик. Вам всего лишь нужно будет" +
            "выполнять действия, которые от вас требует бот.\n" +
            "3) Кнопка: | Удалить тактику 📛 |\n" +
            "-С помощью этой кнопки вы можете удалить свою тактику, используя предложенные фильтры ботом.\n" +
            "4) Кнопка: | Редактировать тактику 📝 |\n" +
            "-С помощью этой кнопки вы можете редактировать свою тактику, используя предложенные фильтры ботом.\n" +
            "5) Кнопка: | Посмотреть KDA 🦾 |\n" +
            "-C помощью этой кнопки вы можете посмотреть свой KDA за текущий акт.\n" +
            "6) Кнопка: | Рандомайзер коллов 🔴 |\n" +
            "-С помощью этой кнопки вы можете получать рандомный колл во время вашей игры на определенной карте." +
            "Коллы нужно будет запрашивать у бота на каждый раун, нажимая соответственную кнопку. Это позволит вашей" +
            "команде играть оригинально каждый раунд, читая колл, который пришел вам в группу. Коллы приходят из" +
            "существующей базы данных, которые пользователи сами создают.",
            
            "Данный телеграм бот был создан программистом @Vladyssslavvv, которому помогал гений @Vasyl_Kor " +
            "в проектировании. Бот будет активно обновляться, пока не достигнет своего логического конца, а именно" +
            "когда автор не будет удовлетворён своим творением.\n" +
            "Бот не собирает конфиденциальную информацию пользователей, кроме НИКнеймов для того, чтобы" +
            "заполнять базу данных тактик!\n" +
            "Если будут идеи для дополнения бота - можете поделиться ими с автором!"
        };
    }
}

