
public class GameMessageClient {

    private GameSyncClient gameSync;
    
    public GameMessageClient (GameSyncClient gameSync) {
        this.gameSync = gameSync;
        GameMessageQueue.Instance.AddListener<PathAssignedMessage>(PathAssigned);
        GameMessageQueue.Instance.AddListener<FormationChangedMessage>(FormationChanged);
        GameMessageQueue.Instance.AddListener<SkillUsedMessage>(SkillUsed);
    }

    public void Destroy() {
        gameSync = null;
        GameMessageQueue.Instance.RemoveListener<PathAssignedMessage>(PathAssigned);
        GameMessageQueue.Instance.RemoveListener<FormationChangedMessage>(FormationChanged);
        GameMessageQueue.Instance.RemoveListener<SkillUsedMessage>(SkillUsed);
    }
    
    private void PathAssigned (PathAssignedMessage msg) {
        gameSync.PathAssigned(msg);
    }
    
    private void FormationChanged (FormationChangedMessage msg) {
        gameSync.FormationChanged(msg);
    }
    
    private void SkillUsed (SkillUsedMessage msg) {
        gameSync.SkillUsed(msg);
    }
}
