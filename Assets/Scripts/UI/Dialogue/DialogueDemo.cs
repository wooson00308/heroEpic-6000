using Scripts.UI;
using UnityEngine;

public class DialogueDemo : MonoBehaviour
{
    public DialogueTreeData data;

    private bool _isRunningDialogue;

    private void OnEnable()
    {
        DialoguePresenter.End += DialogueEndEvent;
    }

    private void OnDisable()
    {
        DialoguePresenter.End -= DialogueEndEvent;
    }

    private void DialogueEndEvent()
    {
        _isRunningDialogue = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRunningDialogue && Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            _isRunningDialogue = true;
            DialoguePresenter.Start?.Invoke(data);
        }
    }
}
