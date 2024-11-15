using System;
using System.Collections.Generic;

public class QuestSystem : Singleton<QuestSystem>
{
    public Action<int, int> UpdateSubQuest;

    private QuestData _data;
    private List<SubQuest> _subQuests = new();
    private QuestStatus _status;

    public QuestStatus Status => _status;

    private void OnEnable()
    {
        UpdateSubQuest += OnUpdateSubQuest;
    }

    private void OnDisable()
    {
        UpdateSubQuest -= OnUpdateSubQuest;
    }

    public void OnActive(QuestData data)
    {
        _data = data;

        _subQuests?.Clear();

        foreach(var subData in _data.subQuests)
        {
            _subQuests.Add(new SubQuest(subData));
        }

        _status = QuestStatus.Active;
    }

    public void OnUpdateSubQuest(int id, int amount)
    {
        if (_status != QuestStatus.Active) return;

        var sub = _subQuests.Find(x => x.Id == id);
        sub?.UpdateAmount(amount);
    }

    public int GetSubQuestAmount(int id)
    {
        var sub = _subQuests.Find(x => x.Id == id);
        return sub.Amount;
    }

    public void TryComplete()
    {
        if(IsComplete())
        {
            _status = QuestStatus.Complete;
        }
    }

    public void TryFail()
    {
        if(IsFail())
        {
            _status = QuestStatus.Fail;
        }
    }

    private bool IsComplete()
    {
        bool isComplete = true;

        foreach(var sub in _subQuests)
        {
            if (sub.Status != QuestStatus.Complete)
            {
                isComplete = false;
                break;
            }
        }

        return isComplete;
    }

    private bool IsFail()
    {
        bool isFail = false;

        foreach (var sub in _subQuests)
        {
            if (sub.Status == QuestStatus.Fail)
            {
                isFail = true;
                break;
            }
        }

        return isFail;
    }
}

public class SubQuest
{
    private SubQuestData _data;
    private QuestStatus _status;
    private int _amount;

    public int Id => _data.id;
    public string DisplayName => _data.displayName;
    public int Amount => _amount;
    public QuestStatus Status => _status;

    public SubQuest(SubQuestData data) 
    {
        _data = data;
        _status = QuestStatus.Active;

        if(!_data.isIncreaseQuest)
        {
            _amount = _data.amount;
        }
    }

    public void UpdateAmount(int amount)
    {
        if(amount > 0)
        {
            IncreaseAmount(amount);
        }
        else if(amount < 0)
        {
            DecreaseAmount(-amount);
        }

        NoticeUI.Show?.Invoke(DisplayName, $"{_amount} / {_data.amount}");
    }

    public void IncreaseAmount(int amount)
    {
        _amount += amount;

        if(_amount >= _data.amount)
        {
            Complete();
        }
    }

    public void DecreaseAmount(int amount)
    {
        _amount -= amount;

        if (_amount <= 0)
        {
            Fail();
        }
    }

    private void Complete()
    {
        _status = QuestStatus.Complete;

        QuestSystem.Instance.TryComplete();
    }

    private void Fail()
    {
        _status = QuestStatus.Fail;

        QuestSystem.Instance.TryFail();
    }

    public override string ToString()
    {
        return $"{DisplayName} {_amount}/{_data.amount}";
    }
}
