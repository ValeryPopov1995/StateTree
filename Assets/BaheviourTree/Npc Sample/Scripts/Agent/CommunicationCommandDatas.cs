using System;
using System.Linq;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateAssetMenu(menuName = "BaheviourTree/Example/Communication Command Datas")]
    public class CommunicationCommandDatas : ScriptableObject
    {
        [Serializable]
        public struct CommunicationCommandData
        {
            public CommunicationCommandType CommandType;
            public Sprite Icon;
            [TextArea(2, 4)] public string Text;
        }

        [SerializeField] private CommunicationCommandData[] _datas;

        [ContextMenu("Create list of data from CommandType enum values")]
        private void CreateListFromEnum()
        {
            var values = Enum.GetValues(typeof(CommunicationCommandType));
            _datas = new CommunicationCommandData[values.Length];
            for (int i = 0; i < _datas.Length; i++)
                _datas[i].CommandType = (CommunicationCommandType)values.GetValue(i);
        }

        public CommunicationCommandData GetCommunicationData(CommunicationCommandType command)
        {
            return _datas.First(datas => datas.CommandType == command);
        }
    }
}