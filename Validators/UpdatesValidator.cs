using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ValoBotV1.BotComponents;

namespace ValoBotV1.Validators;

public static class UpdatesValidator
{
    private static readonly Dictionary<Enum, Func<Update, bool>> CallBackQueryStatesChecking = new()
    {
        {Constants.CreationSteps.Map, IsTypeUpdateMessage},
        {Constants.CreationSteps.AtckDef, IsTypeUpdateMessage},
        {Constants.CreationSteps.ChooseTacticPolicy, IsTypeUpdateMessage},
        
        {Constants.SearchSteps.Selection, IsTypeUpdateMessage},
        
        {Constants.EditSteps.Selection, IsTypeUpdateMessage},
        {Constants.EditSteps.EditTacticMap, IsTypeUpdateMessage},
        {Constants.EditSteps.EditTacticAttckDef, IsTypeUpdateMessage},
        {Constants.EditSteps.EditTacticAccessGroup, IsTypeUpdateMessage},
        {Constants.EditSteps.EditTacticPolicy, IsTypeUpdateMessage},
        
        {Constants.RandomizingSteps.SelectionMap, IsTypeUpdateMessage},
        {Constants.RandomizingSteps.SelectionSide, IsTypeUpdateMessage},
        
        {Constants.TaggerSteps.Selection, IsTypeUpdateMessage},
    };

    public static bool IsUpdateDontHaveUserName(Update update)
    {
        bool answer = true;

        if (IsTypeUpdateMessage(update))
        {
            answer = update.Message.From?.Username == null;
        }
        else
        {
            answer = update.CallbackQuery.From?.Username == null;
        }

        return answer;
    }

    public static bool IsMessageForCallBackQueryState(Context context, Update update)
    {
        if (!CallBackQueryStatesChecking.ContainsKey(context.State))
            return false;

        return CallBackQueryStatesChecking[context.State](update);
    }
    public static bool IsUpdateNullReferenced(Update update)
    {
        bool answer = true;

        if (update.CallbackQuery != null && update.CallbackQuery.Message != null)
            answer = false;
        else if (update.Message != null)
            answer = false;

        return answer;
    }

    public static bool IsOperationCancelled(Update update)
    {
        bool answer = IsTypeUpdateMessage(update) && update.Message.Text == "Отменить задачу ❌"; //bool expression

        return answer;
    }

    public static bool IsTypeUpdateMessage(Update update)
    {
        bool answer = true;

        if (update.Type == UpdateType.CallbackQuery)
            return answer = false;

        return answer;
    }
}