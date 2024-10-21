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

        public int Health { get; set; }
        public float RunSpeed { get; set; }

        private void Awake()
        {
            _stateMachine = GetComponent<BaseStateMachine>();

            data.Setup(this);
        }

        private void Update()
        {
            _stateMachine.OnUpdate();
        }

        public void Run(Vector3 runDir)
        {
            transform.position += RunSpeed * Time.deltaTime * runDir;

            if(runDir.x != 0)
            {
                float rotY = runDir.x > 0 ? 0 : 180;
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
