using System;
using System.Collections.Generic;
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
        ImHealthy,

        ThrowGranade
    }

    [Serializable]
    public class Communication : IInitializable, IDisposable
    {
        public event Action<CommunicationCommandData> OnTell, OnHear;

        [SerializeField] private Speach _speachPrefab;
        [SerializeField] private CommunicationCommandDatas _tellDatas;
        [SerializeField] private CommunicationCommandDatas _hearDatas;
        [SerializeField] private int _tellDistance = 5;
        [SerializeField] private int _minTellDelay = 2;
        private Npc _npc;
        private float _lastTellTime;
        public List<CommunicationCommandType> GettedCommands { get; private set; } = new();

        public bool Initialized { get; private set; }

        public void Init(Npc npc)
        {
            _npc = npc;
            Initialized = true;
        }

        public void TellCommand(CommunicationCommandType command)
        {
            if (!Initialized) return;
            if (Time.time < _lastTellTime + _minTellDelay) return;
            _lastTellTime = Time.time;

            UnityEngine.Object.Instantiate(_speachPrefab).Say(_npc, _tellDatas.GetCommunicationData(command));

            var npcs = _npc.OverlapNpcs(_tellDistance);
            foreach (var npc in npcs)
                npc.Communication.HearComand(command);

            Debug.Log(_npc.name + " tald " + command);
            OnTell?.Invoke(_tellDatas.GetCommunicationData(command));
        }

        public void HearComand(CommunicationCommandType command)
        {
            if (!Initialized) return;

            UnityEngine.Object.Instantiate(_speachPrefab).Say(_npc, _hearDatas.GetCommunicationData(command));
            GettedCommands.Add(command);
            OnHear?.Invoke(_hearDatas.GetCommunicationData(command));
        }

        public void Dispose()
        {
            Initialized = false;
            GettedCommands.Clear();
        }
    }
}