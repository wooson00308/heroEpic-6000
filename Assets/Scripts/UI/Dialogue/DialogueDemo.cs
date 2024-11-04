using Scripts.UI;
using UnityEngine;

public class DialogueDemo : MonoBehaviour
{
    public DialogueTreeData data;

    private bool _isStartDialogue;

    // Update is called once per frame
    void Update()
    {
        if (!_isStartDialogue && Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            _isStartDialogue = true;
            DialoguePresenter.Start?.Invoke(data);
        }
    }
}
