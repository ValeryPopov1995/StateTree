
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [Serializable]
    public class Mover : IInitializable, IDisposable
    {
        public bool IsStopped => Vector3.Distance(_navAgent.transform.position, _navAgent.destination) <= _navAgent.stoppingDistance;
        public bool IsMove => !IsStopped;
        public Target LookTarget { get; private set; }
        public Target MoveTarget { get; private set; }
        public bool Initialized { get; private set; }

        [SerializeField] private NavMeshAgent _navAgent;
        [SerializeField, Min(1)] private int _rotationSpeed = 9;
        private CancellationToken _npcToken;
        private CancellationTokenSource _disposeToken;



        public void Init(CancellationToken token)
        {
            _npcToken = token;
            _disposeToken = new CancellationTokenSource();
            _navAgent.enabled = true;

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
            while (!_npcToken.IsCancellationRequested && !_disposeToken.IsCancellationRequested)
            {
                if (!Target.IsNullOrEmpty(LookTarget))
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
            while (!_npcToken.IsCancellationRequested && !_disposeToken.IsCancellationRequested)
            {
                if (!Target.IsNullOrEmpty(MoveTarget) && _navAgent.destination != MoveTarget)
                    _navAgent.destination = MoveTarget;

                await Task.Delay(100);
            }
        }

        public void Dispose()
        {
            _disposeToken.Cancel();
            LookTarget = MoveTarget = null;
            _navAgent.enabled = false;
            Initialized = false;
        }
    }
}
