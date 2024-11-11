using Scripts.UI;
using System;
using UnityEngine;

public class DialogueSimulator : Singleton<DialogueSimulator>
{
    private DialogueTreeData _data;
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

    public void Initialized(DialogueTreeData data)
    {
        if (_isRunningDialogue) return;
        _isRunningDialogue = true;

        _data = data;
        DialoguePresenter.InitializeDialogue?.Invoke(_data);
    }
}
