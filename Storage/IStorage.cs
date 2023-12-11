namespace ValoBotV1.Storage;

public interface IStorage
{
    public void AddContext(Context context);

    public void DeleteContext(long chatId);

    public bool ContainsContext(long chatId);

    public Context GetContext(long chatId);
}