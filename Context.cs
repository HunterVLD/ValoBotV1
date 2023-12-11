using System.Collections;
using System.Numerics;
using Telegram.Bot.Types;
using ValoBotV1.BotComponents;

namespace ValoBotV1
{
    public class Context
    {
        public readonly Constants.ContextTypes Type;
        public readonly long ChatId; //or user id
        public int LastBotMessageId { get; set; }
        public Enum State { get; set; } // UnicState rl time user

        public List<string> data;

        public Context(Constants.ContextTypes type, int lastBotMessageId, Enum state, long chatId)
        {
            Type = type;
            LastBotMessageId = lastBotMessageId;
            State = state;
            ChatId = chatId;

            data = new();
        }
    }
}

