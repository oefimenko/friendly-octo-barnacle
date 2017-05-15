using UnityEngine;
using System.Collections;

public delegate void FormationProvidedHandler (string formation);
public delegate void SkillProvidedHandler (string skill);
public delegate void SquadSelectedHandler (ISquadModel squad);
public delegate void PathProvidedHandler (Path path);

public class SelectedSquadViewPresenter {

    public event FormationProvidedHandler OnFormationProvided = (string formation) => { };
    public event SkillProvidedHandler OnSkillProvided = (string skill) => { };
    public event SquadSelectedHandler OnSquadSelected = (ISquadModel squad) => { };
    public event PathProvidedHandler OnPathProvided = (Path path) => { };
    public event OnObjectDestroy OnObjectDestroy = () => { };

    private ButtonView[] formations;
    private ButtonView[] skills;
    private InputManager inputManager;

    public SelectedSquadViewPresenter (
        InputManager input,
        ButtonView[] formations,
        ButtonView[] skills
    ) {
        inputManager = input;
        inputManager.OnSquadPathDrawn += OnPathDrawn;
        inputManager.OnSquadClicked += OnSquadClicked;
        for (int i = 0; i < formations.Length; i++) formations[i].OnButtonPressed += OnFormationChange;
        for (int i = 0; i < skills.Length; i++) skills[i].OnButtonPressed += OnSkillUse;
        this.formations = formations;
        this.skills = skills;
        formations[0].OnObjectDestroy += Destory;
    }

    // Events

    private void OnSquadClicked (string name) {
        OnSquadSelected(SquadMonitor.Instance.Get(name));
    }

    private void OnPathDrawn (Path path) {
        OnPathProvided(path);
    }

    private void OnFormationChange(string formation) {
        OnFormationProvided(formation);
    }

    private void OnSkillUse(string skill) {
        OnSkillProvided(skill);
    }

    // Public methods

    public void DisplayFormations (string[] formations, string setFormation) {
        float step = 1f / (formations.Length + 1);
        for (int i = 0; i < formations.Length; i++) {
            this.formations[i].SetValue(formations[i]);
            if (formations.Length == 1) {
                this.formations[i].Rearange(0.5f);
            } else if (formations.Length == 2) {
                this.formations[i].Rearange(i * 0.7f + 0.15f);
            } else {
                this.formations[i].Rearange(step * i);
            }
        }
    }

    public void DisplaySkills(string offensive, string defensive) {
        skills[0].SetValue(offensive);
        skills[1].SetValue(offensive);
    }

    public void Destory () {
        inputManager.OnSquadPathDrawn -= OnPathDrawn;
        inputManager.OnSquadClicked -= OnSquadClicked;
        for (int i = 0; i < formations.Length; i++) formations[i].OnButtonPressed -= OnFormationChange;
        for (int i = 0; i < skills.Length; i++) skills[i].OnButtonPressed -= OnSkillUse;
        formations[0].OnObjectDestroy -= Destory;
        this.formations = null;
        this.skills = null;
        OnObjectDestroy();
    }
}
