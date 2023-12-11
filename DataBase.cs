using System.Text.RegularExpressions;
using MongoDB.Driver;
using MongoDB.Entities;
using ValoBotV1.BotComponents;
using ValoBotV1.Validators;

namespace ValoBotV1
{
    public class DataBase
    {
        private readonly int _dbPort = 27017;
        private readonly string _mongoUrl = "localhost";
        private readonly string _dbName = "ValorantBot";

        public async Task ExecuteDataBase()
        {
            await DB.InitAsync(_dbName, _mongoUrl, _dbPort);
        }

        public async Task CheckDataAndSet(List<string> data)
        {
            var user = (await DB.Find<Users>().ManyAsync(a => a.UserName == data.ElementAt(0)))[0];

            var tactic = ComposeAndCreateTactic(data, user).Result;

            if (tactic.TacticPolicy == "Private")
            {
                string[] accessibleUsers = Regex.Split(data.ElementAt(3), @"[^0-9a-zA-Z_]+");

                int iteration = 0;
                foreach (var accessibleUser in accessibleUsers)
                {
                    if (await DBValidator.IsUserExist(accessibleUser))
                    {
                        user = (await DB.Collection<Users>().FindAsync(x => x.UserName == accessibleUser)).ToList()[0];
                        await user.AccessibleTactics.AddAsync(tactic);
                    }
                }
            }
            
            Console.WriteLine("New data Setted");
        }

        public async Task CreateUser(string userName, long chatId)
        {
            var user = new Users
            {
                UserName = userName,
                UserTelegramChatId = chatId,
                UserStatsLink = "none",
                TagMessage = "none"
            };

            await user.SaveAsync();
            
            Console.WriteLine("New User Created");
        }

        public async Task ChangeUserName(string newName, long userId)
        {
            await DB.Update<Users>()
                .Match(x => x.UserTelegramChatId == userId)
                .Modify(x => x.Set(a => a.UserName, newName))
                .ExecuteAsync();
        }
        
        public async Task SetAndChangeUserTagMessage(long userId, string tagMessage)
        {
            await DB.Update<Users>()
                .Match(x => x.UserTelegramChatId == userId)
                .Modify(x => x.Set(a => a.TagMessage, tagMessage))
                .ExecuteAsync();
        }
        
        public async Task<string> GetTagMessage(long userId)
        {
            string result = "";

            var user = (await DB.Collection<Users>()
                .FindAsync(x => x.UserTelegramChatId == userId)).ToList()[0];

            return result = user.TagMessage;
        }
        
        private async Task<MainTactics> ComposeAndCreateTactic(List<string> data, Users user)
        {
            var tactic = new MainTactics
            {
                Creator = user,
                TacticName = data.ElementAt(1),
                TacticPolicy = data.ElementAt(2),
                //access group skipped
                Map = data.ElementAt(4),
                AtackOrDefense = data.ElementAt(5),
                ASite = data.ElementAt(6),
                BSite = data.ElementAt(7),
                CSite = data.ElementAt(8),
                DescriptionOpt = data.ElementAt(9),
                SomePhotoLinkOpt = data.ElementAt(10),
            };
            
            await tactic.SaveAsync();
            await user.Tactics.AddAsync(tactic);
            
            return tactic;
        }

        public async Task<string> GetMyTacticsByMap(List<string> data)
        {
            string result = "";

            var user = (await DB.Collection<Users>().FindAsync(x => x.UserName == data.ElementAt(0))).ToList()[0];
            var userTactics = await user.Tactics.ChildrenQueryable().ToListAsync();
            var usersTacticWithAccess = await user.AccessibleTactics.ChildrenQueryable().ToListAsync();

            userTactics.AddRange(usersTacticWithAccess);

            List<MainTactics> tacticByMap = new();

            foreach (var tactic in userTactics)
            {
                if (tactic.Map == data.ElementAt(1) && tactic.AtackOrDefense == data.ElementAt(2))
                {
                    tacticByMap.Add(tactic);
                }
            }
            
            Random random = new();
            int randIndex = random.Next(tacticByMap.Count);
            
            Console.WriteLine(tacticByMap.Count);
            
            result = await InterfaceServices.RefactorRandomizingData(tacticByMap, randIndex);
            
            return result;
        }
        public async Task<string> GetCollectionItemByFilter(List<string> data)
        {
            string resultOfSearch = "";

            string userName = data.ElementAt(0);
            string filterType = data.ElementAt(1);

            List<MainTactics> items;
            
            switch (filterType)
            {
                case "SearchByName":
                    if (!await DBValidator.IsSearchDataCurrent(data))
                        return $"В публичной базе данных нету тактики * {data} * ! 🟥";

                    items = await DB.Find<MainTactics>().ManyAsync(x => x.TacticName == data.ElementAt(2));

                    resultOfSearch = await InterfaceServices.RefactorTextAfterSearching(items, filterType);
                    break;
                case "GetAll":
                    if (!await DBValidator.IsSearchDataCurrent(data))
                        return $"В базе данных нету тактик! 🟥";
                    
                    items = await DB.Find<MainTactics>().ManyAsync(x => x.TacticPolicy == "Public");

                    resultOfSearch = await InterfaceServices.RefactorTextAfterSearching(items, filterType);

                    break;
                case "GetMyTactics":
                    Console.WriteLine(data);
                    if (!await DBValidator.IsSearchDataCurrent(data))
                        return "У вас нету собственно созданых тактик или доступа к другим! 🟥";
                    
                    var user = (await DB.Find<Users>().ManyAsync(x => x.UserName == userName))[0];
                    var userTactics = await user.Tactics.ChildrenQueryable().ToListAsync();
                    var usersTacticWithAccess = await user.AccessibleTactics.ChildrenQueryable().ToListAsync();

                    userTactics.AddRange(usersTacticWithAccess);
                    
                    resultOfSearch = await InterfaceServices.RefactorTextAfterSearching(userTactics, filterType);

                    break;
                default:
                    return "Ошибка поиска";
            }

            return resultOfSearch;
        }

        public async Task DeleteItem(string data) 
        {
            await DB.DeleteAsync<MainTactics>(x => x.TacticName == data);
        }
        
        public async Task DeleteTacticUserAccess(string tacticName, string userName) 
        {
            var user = (await DB.Collection<Users>().FindAsync(x=> x.UserName == userName))
                .ToListAsync().Result[0];

            var usersTacticsWithAccess = await user.AccessibleTactics.ChildrenQueryable().ToListAsync();

            foreach (var tacticItem in usersTacticsWithAccess)
            {
                if (tacticItem.TacticName == tacticName)
                {
                    await tacticItem.DeleteAsync();
                }
            }
        }
        
        public async Task AddTacticUserAccess(string tacticName, string userName) 
        {
            var user = (await DB.Collection<Users>().FindAsync(x=> x.UserName == userName))
                .ToListAsync().Result[0];
            
            var tactic =(await DB.Collection<MainTactics>().FindAsync(x=> x.TacticName == tacticName))
                .ToListAsync().Result[0];

            await user.AccessibleTactics.AddAsync(tactic);
        }

        public async Task CheckFilterAndUpdate(string tacticName, string userName, string filterType,
            string data)
        {
            switch (filterType)
            {
                case "EditTacticName":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.TacticName, data))
                        .ExecuteAsync();
                    break;
                case "EditTacticPolicy":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.TacticPolicy, data))
                        .ExecuteAsync();
                    break;
                case "EditTacticMap":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.Map, data))
                        .ExecuteAsync();
                    break;
                case "EditTacticAttckDef":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.AtackOrDefense, data))
                        .ExecuteAsync();
                    break;
                case "EditTacticSiteA":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.ASite, data))
                        .ExecuteAsync();
                    break;
                case "EditTacticSiteB":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.BSite, data))
                        .ExecuteAsync();
                    break;
                case "EditTacticSiteC":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.CSite, data))
                        .ExecuteAsync();
                    break;
                case "EditTacticDescription":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.DescriptionOpt, data))
                        .ExecuteAsync();
                    break;
                case "EditTacticPhoto":
                    await DB.Update<MainTactics>()
                        .Match(x => x.TacticName == tacticName)
                        .Modify(x => x.Set(a => a.SomePhotoLinkOpt, data))
                        .ExecuteAsync();
                    break;
                default:
                    Console.WriteLine("Error update");
                    break;
            }
        }
        
        public class MainTactics : Entity
        {
            public One<Users> Creator { get; set; }
            
            [OwnerSide]
            public Many<Users>? UsersCanRead { get; set; }

            public string? TacticName { get; set; }
             
            public string? TacticPolicy { get; set; }

            public string? Map { get; set; }

            public string? AtackOrDefense { get; set; }
    
            public string? ASite { get; set; }
    
            public string? BSite { get; set; }
            
            public string? CSite { get; set; }
            
            public string? DescriptionOpt { get; set; }
    
            public string? SomePhotoLinkOpt { get; set; }

            public MainTactics()
            {
                this.InitManyToMany(() => UsersCanRead, user => user.AccessibleTactics);
            }
        }
        
        public class Users : Entity
        {
            public string UserName { get; set; }
            
            public long UserTelegramChatId { get; set; }
            
            public string? UserStatsLink { get; set; }
            
            public Many<MainTactics> Tactics { get; set; }
            
            [InverseSide]
            public Many<MainTactics> AccessibleTactics { get; set; }
            
            public string? TagMessage { get; set; }

            public Users()
            {
                this.InitOneToMany(() => Tactics);
                this.InitManyToMany(() => AccessibleTactics, tactic => tactic.UsersCanRead);
            }
        }
    }
}

