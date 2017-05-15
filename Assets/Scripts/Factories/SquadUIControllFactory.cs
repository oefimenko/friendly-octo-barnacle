using UnityEngine;
using UnityEngine.UI;

public class SquadUIControllFactory {

	public static SelectedSquadController Create (InputManager manager) {
        GameObject gui = GameObject.FindGameObjectWithTag("GUI");
        Button[] buttons = gui.GetComponentsInChildren<Button>();
        ButtonView[] formations = new ButtonView[3];
        ButtonView[] skills = new ButtonView[2];
        int formI = 0;
        int skillI = 0;

        for (int i = 0; i < buttons.Length; i++) {
            string name = buttons[i].gameObject.name;
            if (name.Contains("Formation")) {
                ButtonView view = buttons[i].gameObject.AddComponent<ButtonView>();
                formations[formI] = view;
                formI++;
            } else if (name.Contains("Skill")) {
                ButtonView view = buttons[i].gameObject.AddComponent<ButtonView>();
                skills[skillI] = view;
                skillI++;
            }
        }

        SelectedSquadViewPresenter viewPresenter = new SelectedSquadViewPresenter(manager, formations, skills);
        return new SelectedSquadController(viewPresenter);
    }
}
