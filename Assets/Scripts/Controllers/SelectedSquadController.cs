using UnityEngine;

public class SelectedSquadController {

    private ISquadModel model;
    private SelectedSquadViewPresenter viewPresenter;

    public SelectedSquadController(SelectedSquadViewPresenter nView) {
        viewPresenter = nView;
        viewPresenter.OnFormationProvided += OnFormationProvided;
        viewPresenter.OnPathProvided += OnPathProvided;
        viewPresenter.OnSkillProvided += OnSkillProvided;
        viewPresenter.OnSquadSelected += OnSquadSelected;
        viewPresenter.OnObjectDestroy += Destroy;
    }

    public void Destroy () {
        viewPresenter.OnFormationProvided -= OnFormationProvided;
        viewPresenter.OnPathProvided -= OnPathProvided;
        viewPresenter.OnSkillProvided -= OnSkillProvided;
        viewPresenter.OnSquadSelected -= OnSquadSelected;
        viewPresenter.OnObjectDestroy -= Destroy;
        model = null;
        viewPresenter = null;
    }

    private void OnFormationProvided (string formation) {
        GameMessageQueue.Instance.QueueEvent(new FormationChangedMessage(model, formation));
    }

    private void OnSkillProvided (string skill) {
        GameMessageQueue.Instance.QueueEvent(new SkillUsedMessage(model, skill));
        Debug.Log("Called skill is not implemented: " + skill);
    }

    private void OnSquadSelected (ISquadModel model) {
        if (model != null) {
            DisplaySquadGUI(model);
        } else {
            viewPresenter.HideSquadGUI();
        }
    }

    private void OnPathProvided (Path path) {
        GameMessageQueue.Instance.QueueEvent(new PathAssignedMessage(model, path));
    }

    private void DisplaySquadGUI (ISquadModel model) {
        this.model = model;
        string[] keys = new string[model.Formations.Count];
        model.Formations.Keys.CopyTo(keys, 0);
        viewPresenter.DisplayFormations(keys, model.Formation);
        viewPresenter.DisplaySkills(model.OffensiveSkill, model.DefensiveSkill);
    }
}
