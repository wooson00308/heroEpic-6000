using Scripts.StateMachine;
using Scripts.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts
{
    public enum Team
    {
        Player, 
        Monster
    }

    public class Unit : MonoBehaviour, IStats
    {
        private SpriteRenderer _modelRenderer;
        private NavMeshAgent _agent;

        private BaseStateMachine _stateMachine;
        private bool _isHitable = true;

        private bool _isRunningDialogue;

        public BaseStateMachine StateMachine => _stateMachine;

        [Header("Config")]
        public StatsData data;
        public Transform model;
        public Team team;

        [Header("HitBox")]
        public HitBoxCaster hitBoxCaster;
        public HitBoxReceiver hitBoxReceiver;

        [Header("Another")]
        public Animator _emotion;
        public AttackEffect _hitEffect;

        public string DisplayName { get; set; }
        public int Health { get; set; }
        public float RunSpeed { get; set; }
        public int Damage { get; set; }

        public bool IsDeath { get; set; }

        [Range(0, 1f)]
        public float _backMoveDecreasePercent;

        private void OnEnable()
        {
            IsDeath = false;

            hitBoxReceiver.OnHitReceive += OnHitEvent;
            DialoguePresenter.Start += OnDialogueStart;
            DialoguePresenter.End += OnDialogueEnd;
        }

        private void OnDisable()
        {
            hitBoxReceiver.OnHitReceive -= OnHitEvent;
            DialoguePresenter.Start -= OnDialogueStart;
            DialoguePresenter.End -= OnDialogueEnd;
        }

        private void OnDialogueStart()
        {
            _isRunningDialogue = true;
        }

        private void OnDialogueEnd()
        {
            _isRunningDialogue = false;
        }

        private void OnHitEvent(HitBoxData data)
        {
            if (IsDeath) return;
            if (!_isHitable) return;
            StartCoroutine(OnHitDelay());

            OnHit(data.HitBoxCaster.GetComponentInParent<Unit>());
        }

        private IEnumerator OnHitDelay()
        {
            if (!_isHitable) yield break;
            _isHitable = false;

            yield return new WaitForSeconds(0.08f);
            _isHitable = true;
        }

        private void Awake()
        {
            _stateMachine = GetComponent<BaseStateMachine>();
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;

            _modelRenderer = model.GetComponent<SpriteRenderer>();

            data.Setup(this);
        }

        private void Update()
        {
            _stateMachine.OnUpdate();
            UpdateSortingOrder();
        }

        private void UpdateSortingOrder()
        {
            if (_modelRenderer != null)
            {
                _modelRenderer.sortingOrder = Mathf.RoundToInt(-model.position.y * 100);
            }
        }

        public void Emotion(EmotionType type)
        {
            _emotion.CrossFade(type.ToString(), 0);
        }

        public void Stop(Vector3 dir, bool isRotation = true)
        {
            _agent.isStopped = true;
            if (isRotation)
            {
                Rotation(dir);
            }
        }

        public void RunAgent(Vector3 runDir)
        {
            _agent.isStopped = false;
            _agent.Move(RunSpeed * Time.deltaTime * runDir);
            Rotation(runDir);
        }

        public void RunAgentToTarget(Transform target)
        {
            _agent.isStopped = false;
            _agent.speed = RunSpeed;
            _agent.SetDestination(target.position);
        }

        public void ResetPath()
        {
            _agent.ResetPath();
        }

        public void BackMoveAgent(Vector3 dir)
        {
            _agent.isStopped = true;
            var runSpeed = RunSpeed - RunSpeed * _backMoveDecreasePercent;

            _agent.Move(runSpeed * Time.deltaTime * -dir);
            Rotation(dir);
        }

        public void Rotation(Vector3 rotDir)
        {
            if (rotDir.x != 0)
            {
                float rotY = rotDir.x > 0 ? 0 : 180;
                model.rotation = Quaternion.Euler(0, rotY, 0);
            }
        }

        public void OnHit(Unit attacker)
        {
            _stateMachine.OnHit();

            if (_isRunningDialogue) return;

            int criticalRandom = Random.Range(0, 10);

            int criticalDamage = attacker.Damage * 2;

            bool isCritical = criticalRandom > 8;

            if (isCritical)
            {
                Health -= criticalDamage;
            }
            else
            {
                Health -= attacker.Damage;
            }

            _hitEffect.OnAttack(isCritical);

            HealthBarPresenter.Update?.Invoke(this);

            if (Health <= 0)
            {
                OnDeath(attacker);
            }
        }

        public void OnDeath(Unit attacker)
        {
            if (IsDeath) return;
            IsDeath = true;

            hitBoxCaster.enabled = false;
            hitBoxReceiver.enabled = false;

            _stateMachine.OnDeath();

            QuestSystem.Instance.UpdateSubQuest?.Invoke(data.id, 1);
        }
    }
}
