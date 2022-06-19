using TMPro;
using UnityEngine.EventSystems;

public class AutoSelectFieldTextOnEnable : UIBehaviour
{
    // This class is used on the Text Input fields to automatically 'activate' them for input upon instantiation.
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