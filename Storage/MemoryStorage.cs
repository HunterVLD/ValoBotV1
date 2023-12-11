 using ValoBotV1.Storage;

 namespace ValoBotV1
{
    public class MemoryStorage : IStorage
    {
        private readonly Dictionary<long, Context> _context;

        public MemoryStorage()
        {
            _context = new();
        }

        public void AddContext(Context context)
        {
            _context.Add(context.ChatId, context);
        }

        public void DeleteContext(long chatId)
        {
            _context.Remove(chatId);
        }

        public bool ContainsContext(long chatId)
        {
            return _context.ContainsKey(chatId);
        }

        public Context GetContext(long chatId)
        {
            return _context[chatId];
        }
    }
}