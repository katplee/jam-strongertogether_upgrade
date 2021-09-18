using TMPro;

public class DP_UIName : UIObject
{
    private DP_UIDragon parent;

    private TMP_Text text;

    public override string Label
    {
        get { return GetType().Name; }
    }

    private void Start()
    {
        text = GetComponent<TMP_Text>();

        if (transform.parent.TryGetComponent(out parent))
        {
            parent.DeclareThis(Label, this);
        }
    }

    public void ChangeText(string name)
    {
        text.text = name;
    }
}
