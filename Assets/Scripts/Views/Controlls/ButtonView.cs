using UnityEngine;
using UnityEngine.UI;

public delegate void ButtonPressedHandler (string value);

public class ButtonView : MonoBehaviour {

    public event ButtonPressedHandler OnButtonPressed = (string value) => { };
    public event OnObjectDestroy OnObjectDestroy = () => { };
    private Button button;
    private Text text;
    private string buttonName;
    new private RectTransform transform;

    void Start () {
        button = gameObject.GetComponent<Button>();
        text = gameObject.GetComponentInChildren<Text>();
        button.onClick.AddListener(OnClickEvent);
        transform = GetComponent<RectTransform>();
        Hide();
    }

    public void Hide () {
        gameObject.SetActive(false);
    }

    public void SetValue (string value) {
        buttonName = value;
        text.text = value;
        Show();
    }

    public void Show () {
        gameObject.SetActive(true);
    }

    public void Rearange (float yPosition) {
        transform.anchorMin = new Vector2(transform.anchorMin.x, yPosition);
        transform.anchorMax = new Vector2(transform.anchorMin.x, yPosition);
    }

    private void OnClickEvent () {
        OnButtonPressed(buttonName);
    }

    public void OnDestroy () {
        OnObjectDestroy();
        button = null;
        text = null;
        transform = null;
    }
}
