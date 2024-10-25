using Scripts.StateMachine;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts
{
    public class Unit : MonoBehaviour, IStats
    {
        private SpriteRenderer _modelRenderer;
        private NavMeshAgent _agent;

        private BaseStateMachine _stateMachine;
        private bool _isHitable = true;

        [Header("Config")]
        public StatsData data;
        public Transform model;

        [Header("HitBox")]
        public HitBoxCaster hitBoxCaster;
        public HitBoxReceiver hitBoxReceiver;

        public int Health { get; set; }
        public float RunSpeed { get; set; }
        public int Damage { get; set; }

        public bool IsDeath { get; set; }

        [Range(0, 1f)]
        public float _backMoveDecreasePercent;

        private void OnEnable()
        {
            hitBoxReceiver.OnHitReceive += OnHitEvent;
        }

        private void OnDisable()
        {
            hitBoxReceiver.OnHitReceive -= OnHitEvent;
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

        public void BackMoveAgent(Vector3 dir)
        {
            _agent.isStopped = false;
            _agent.speed = RunSpeed - RunSpeed * _backMoveDecreasePercent;
            _agent.SetDestination(-dir);
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

            Health -= attacker.Damage;

            if (Health <= 0)
            {
                OnDeath(attacker);
            }
        }

        public void OnDeath(Unit attacker)
        {
            hitBoxCaster.enabled = false;
            hitBoxReceiver.enabled = false;

            _stateMachine.OnDeath();
        }
    }
}
