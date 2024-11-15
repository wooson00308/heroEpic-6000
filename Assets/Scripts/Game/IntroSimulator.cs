using Scripts;
using Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSimulator : MonoBehaviour
{
    public DialogueView _dialogueView;
    public DialogueTreeData _introDialogue;
    public Unit _unit;
    public List<Unit> _goblins;
    public QuestData _introQuest;

    public Transform _introMovePoint;

    private bool _canNextDialogue;

    private void Start()
    {
        StartCoroutine(StartIntro());
    }

    private void Update()
    {
        if (!_canNextDialogue) return;
        if(Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            _dialogueView.OnNextDialogue();
        }
    }

    private IEnumerator StartIntro()
    {
        yield return new WaitForEndOfFrame();

        Intro();
    }

    public void Intro()
    {
        DialoguePresenter.Update += UpdateIntroAction;
        DialoguePresenter.End += EndIntroAction;

        DialogueSimulator.Instance.Initialized(_introDialogue);
    }

    private void OnDisable()
    {
        DialoguePresenter.Update -= UpdateIntroAction;
        DialoguePresenter.End -= EndIntroAction;
    }

    private void UpdateIntroAction(int index)
    {
        _canNextDialogue = false;

        if (index == 0)
        {
            _unit.Emotion(EmotionType.Whisper);
            _canNextDialogue = true;
        }
        if(index == 1)
        {
            StartCoroutine(Index1Logic());
        }
        if(index == 2)
        {
            StartCoroutine(Index2Logic());
        }
        if(index == 3)
        {
            foreach (var goblin in _goblins)
            {
                goblin.Emotion(EmotionType.Anger);
                goblin.StateMachine.Animator.CrossFade("Pattern 1", 0);
            }

            _canNextDialogue = true;
        }
    }

    private IEnumerator Index1Logic()
    {
        _unit.Emotion(EmotionType.Whisper);

        yield return new WaitForSeconds(1.5f);

        _unit.RunAgentToTarget(_introMovePoint);

        var unitAnimator = _unit.StateMachine.Animator;
        unitAnimator.CrossFade("Run", 0);

        while (Vector2.Distance(_unit.transform.position, _introMovePoint.position) > 0.1f)
        {
            yield return null;
        }

        unitAnimator.CrossFade("Idle", 0);
        _unit.ResetPath();

        foreach (var goblin in _goblins)
        {
            goblin.Emotion(EmotionType.Sigh);
        }

        _canNextDialogue = true;
    }

    private IEnumerator Index2Logic()
    {
        _unit.Emotion(EmotionType.Dispirit);
        yield return new WaitForSeconds(1f); 
        _unit.StateMachine.Animator.CrossFade("Attack", 0);

        _canNextDialogue = true;
    }

    private void EndIntroAction()
    {
        DialoguePresenter.Update -= UpdateIntroAction;
        DialoguePresenter.End -= EndIntroAction;

        QuestSystem.Instance.OnActive(_introQuest);
    }
}
