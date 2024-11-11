using Scripts;
using Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDemo : MonoBehaviour
{
    public DialogueTreeData _introDialogue;
    public Unit _unit;
    public List<Unit> _goblins;

    public Transform _introMovePoint;

    private void Start()
    {
        StartCoroutine(StartIntro());
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
        if(index == 0)
        {
            StartCoroutine(Index0Logic());
        }
        if(index == 1)
        {
            _unit.Emotion(EmotionType.Whisper);
        }
        if(index == 2)
        {
            foreach (var goblin in _goblins)
            {
                goblin.Emotion(EmotionType.Anger);
            }
        }
    }

    private IEnumerator Index0Logic()
    {
        _unit.Emotion(EmotionType.Whisper);

        yield return new WaitForSeconds(.8f);

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
    }

    private void EndIntroAction()
    {
        DialoguePresenter.Update -= UpdateIntroAction;
        DialoguePresenter.End -= EndIntroAction;
    }
}
