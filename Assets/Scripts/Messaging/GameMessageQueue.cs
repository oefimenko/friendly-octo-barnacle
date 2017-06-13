
public class GameMessageQueue {

    private static MessageQueue instance;

    public static MessageQueue Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = new MessageQueue();
                return instance;
            }
        }
    }

    public static void Purge()
    {
        if (instance != null)
        {
            instance.Destroy();
            instance = null;
        }
    }
}