
public class GameSyncClient {

    public GameSyncClient () { }

    public void Destroy() { }

    public void PathAssigned (PathAssignedMessage msg) {
        GameSyncQueue.Instance.QueueEvent(msg);
    }
    
    public void FormationChanged (FormationChangedMessage msg) {
        GameSyncQueue.Instance.QueueEvent(msg);
    }
    
    public void SkillUsed (SkillUsedMessage msg) {
        GameSyncQueue.Instance.QueueEvent(msg);
    }
}
