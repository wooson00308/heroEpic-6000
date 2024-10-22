using Scripts.StateMachine;
using UnityEngine;

namespace Scripts
{
    public class Unit : MonoBehaviour, IStats
    {
        private BaseStateMachine _stateMachine;

        [Header("Config")]
        public StatsData data;
        public Transform model;

        [Header("HitBox")]
        public HitBoxCaster hitBoxCaster;
        public HitBoxReceiver hitBoxReceiver;

        public int Health { get; set; }
        public float RunSpeed { get; set; }

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
            OnHit(null);
        }

        private void Awake()
        {
            _stateMachine = GetComponent<BaseStateMachine>();

            data.Setup(this);
        }

        private void Update()
        {
            _stateMachine.OnUpdate();
        }

        public void Run(Vector3 runDir, bool isRotation = true)
        {
            transform.position += RunSpeed * Time.deltaTime * runDir;

            if (isRotation)
            {
                Rotation(runDir);
            }
        }

        public void BackMove(Vector3 dir)
        {
            transform.position += (RunSpeed - RunSpeed * _backMoveDecreasePercent) * Time.deltaTime * -dir;
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
        }

        public void OnDeath(Unit attacker)
        {
            _stateMachine.OnDeath();
        }
    }
}
