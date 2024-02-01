using System;
using UnityEngine;
using static ValeryPopov.Common.StateTree.NpcSample.CommunicationCommandDatas;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public enum CommunicationCommandType
    {
        None,
        SomthingWrong,
        EnemyDetected,
        Reloading,
        NeedHelp, // medic!
        CoverMe,
        ImHealthy
    }

    [Serializable]
    public class Communication : IInitializable, IDisposable
    {
        public event Action<CommunicationCommandData> OnTell, OnHear;

        [SerializeField] private Speach _speachPrefab;
        [SerializeField] private CommunicationCommandDatas _tellDatas;
        [SerializeField] private CommunicationCommandDatas _hearDatas;
        [SerializeField] private int _tellDistance = 5;
        private Npc _npc;

        public bool Initialized { get; private set; }

        public void Init(Npc npc)
        {
            _npc = npc;
            Initialized = true;
        }

        public void Tell(CommunicationCommandType command)
        {
            if (!Initialized) return;

            UnityEngine.Object.Instantiate(_speachPrefab).Say(_npc, _tellDatas.GetCommunicationData(command));

            var npcs = _npc.OverlapNpcs(_tellDistance);
            foreach (var npc in npcs)
                npc.Communication.Hear(command);

            OnTell?.Invoke(_tellDatas.GetCommunicationData(command));
        }

        public void Hear(CommunicationCommandType command)
        {
            if (!Initialized) return;

            UnityEngine.Object.Instantiate(_speachPrefab).Say(_npc, _hearDatas.GetCommunicationData(command));
            OnHear?.Invoke(_hearDatas.GetCommunicationData(command));
        }

        public void Dispose()
        {
            Initialized = false;
        }
    }
}