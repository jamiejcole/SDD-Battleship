using TMPro;
using UnityEngine.EventSystems;

public class AutoSelectFieldTextOnEnable : UIBehaviour
{
    private TMP_InputField _field;

    protected override void Awake()
    {
        base.Awake();
        _field = GetComponent<TMP_InputField>();
        _field.onFocusSelectAll = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _field.Select();
    }
}