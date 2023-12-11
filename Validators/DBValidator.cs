using MongoDB.Driver;
using MongoDB.Entities;

namespace ValoBotV1.Validators;

public static class DBValidator
{
    public static async Task<bool> IsUserExist(string userName)
    {
        bool answer = (await DB.Collection<DataBase.Users>().
            FindAsync(x => x.UserName == userName)).AnyAsync().Result;

        return answer;
    } 
    public static async Task<bool> IsUserAlreadyCreated(long chatId)
    {
        bool answer = (await DB.Collection<DataBase.Users>().
            FindAsync(x => x.UserTelegramChatId == chatId)).AnyAsync().Result;

        return answer;
    } 
    
    public static async Task<bool> IsUserHaveCreatedAndAccessTacticByMap(long chatId, string map, string side)
    {
        bool answer = false;
        
        var user = (await DB.Collection<DataBase.Users>().FindAsync(x => x.UserTelegramChatId == chatId)).ToList()[0];
        
        var createdTactics = await user.Tactics.ChildrenQueryable().ToListAsync();
        var accessTactics = await user.AccessibleTactics.ChildrenQueryable().ToListAsync();
        
        createdTactics.AddRange(accessTactics);

        foreach (var tactic in createdTactics)
        {
            if (tactic.Map == map && tactic.AtackOrDefense == side)
            {
                return answer = true;
            }
        }

        return answer;
    }

    public static async Task<bool> IsUserChangeName(string userName, long chatId)
    {
        bool answer = (await DB.Collection<DataBase.Users>().
            FindAsync(x => x.UserTelegramChatId == chatId && x.UserName != userName)).AnyAsync().Result;

        return answer;
    } 
    
    public static async Task<bool> IsUserHaveTagMessage(long userId)
    {
        bool answer = (await DB.Collection<DataBase.Users>().
            FindAsync(x => x.UserTelegramChatId == userId && x.TagMessage != "none")).AnyAsync().Result;

        return answer;
    } 
    
    public static async Task<bool> IsUserHaveAccessToTactic(string userName, string tacticDataSearch = "", 
        string? filterType = null)
    {
        bool answer = false;
        var user = (await DB.Collection<DataBase.Users>().FindAsync(x => x.UserName == userName)).ToList()[0]; //todo change to ChatId
        
        if (filterType == "GetMyTactics")
        {
            answer = await user.AccessibleTactics.ChildrenQueryable().AnyAsync();
            return answer;
        }
        
        var accessibleUserTactic = await user.AccessibleTactics.ChildrenQueryable().ToListAsync();

        foreach (var tactic in accessibleUserTactic)
        {
            if (tactic.TacticName == tacticDataSearch)
            {
                return answer = true;
            }
        }

        return answer;
    } //great completed
    
    public static async Task<bool> IsUserHaveOwnTactic(string userName, string tacticDataSearch = "", string? filterType = "")
    {
        bool answer = false;

        var user = (await DB.Find<DataBase.Users>().ManyAsync(a => a.UserName == userName))[0];
        
        if (!await user.Tactics.ChildrenQueryable().AnyAsync())
            return answer;
        else if (filterType == "GetMyTactics")
            return answer = true;
        
        var userTactics = await user.Tactics.ChildrenQueryable().ToListAsync();

        foreach (var tactic in userTactics)
        {
            if (tactic.TacticName == tacticDataSearch)
            {
                answer = true;
                break;
            }
        }

        return answer;
    } //great completed
    
    public static async Task<bool> IsTacticPrivacyPublic(string tacticName)
    {
        Console.WriteLine("NOOOOOOO");
        bool answer = false;
        
        var tactic = (await DB.Find<DataBase.MainTactics>().
            ManyAsync(a => a.TacticName == tacticName))[0];

        if (tactic.TacticPolicy == "Public")
            answer = true;
        
        return answer;
    }
    
    public static async Task<bool> IsTacticNameAlreadyExist(string data)
    {
        bool answer = (await DB.Collection<DataBase.MainTactics>().FindAsync(a => a.TacticName == data)).AnyAsync().Result;
        
        return answer;
    }
    
    public static async Task<bool> IsSearchDataCurrent(List<string> data)
    {
        string userName = data.ElementAt(0);
        string tacticDataFilter = data.ElementAt(1);
        
        bool answer = false;
        switch (tacticDataFilter) //filter
        {
            case "SearchByName":
                answer = await IsTacticPrivacyPublic(data.ElementAt(2)) || 
                         await IsUserHaveOwnTactic(userName, data.ElementAt(2)) || 
                         await IsUserHaveAccessToTactic(userName, data.ElementAt(2));
                         
                break;
            case "GetMyTactics":
                answer = await IsUserHaveOwnTactic(userName, filterType: tacticDataFilter) || 
                         await IsUserHaveAccessToTactic(userName, filterType: tacticDataFilter);
                break;
            case "GetAll":
                answer = (await DB.Collection<DataBase.MainTactics>().FindAsync(
                    x => x.TacticPolicy == "Public")).AnyAsync().Result;
                break;
            default:
                Console.WriteLine("Ошибка Подтверждения поиска данных!");
                break;
        }

        return answer;
    }
    
    
}