
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [Serializable]
    public class Mover : IInitializable
    {
        public bool IsStopped => Vector3.Distance(_navAgent.transform.position, _navAgent.destination) <= _navAgent.stoppingDistance;
        public bool IsMove => !IsStopped;
        public Target LookTarget { get; private set; }
        public Target MoveTarget { get; private set; }
        public bool Initialized { get; private set; }

        [SerializeField] private NavMeshAgent _navAgent;
        [SerializeField, Min(1)] private int _rotationSpeed = 9;
        private CancellationToken _token;



        public void Init(CancellationToken token)
        {
            _token = token;
            _navAgent.updateRotation = false;

            if (_navAgent.stoppingDistance < 1)
            {
                Debug.Log("nav agent stop distance < 1");
                _navAgent.stoppingDistance = 1;
            }

            LookCircle();
            MoveCircle();

            Initialized = true;
        }

        public void LookAt(Target target)
        {
            LookTarget = target;
        }

        public void MoveTo(Target target)
        {
            MoveTarget = target;
        }

        private async void LookCircle()
        {
            while (!_token.IsCancellationRequested)
            {
                if (LookTarget != null)
                {
                    if (_navAgent.updateRotation)
                        _navAgent.updateRotation = false;

                    Vector3 lookTarget = new(LookTarget.Position.x, _navAgent.transform.position.y, LookTarget.Position.z);
                    _navAgent.transform.rotation = Quaternion.RotateTowards(
                        _navAgent.transform.rotation,
                        Quaternion.LookRotation(lookTarget - _navAgent.transform.position),
                        _rotationSpeed * Time.deltaTime);
                }
                else if (!_navAgent.updateRotation)
                    _navAgent.updateRotation = true;

                await Task.Yield();
            }
        }

        private async void MoveCircle()
        {
            while (!_token.IsCancellationRequested)
            {
                if (MoveTarget != null && _navAgent.destination != MoveTarget)
                    _navAgent.destination = MoveTarget;

                await Task.Delay(100);
            }
        }
    }
}
