using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ValeryPopov.Common.StateTree.NpcSample.CommunicationCommandDatas;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Speach : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _delay = 2;

        public async void Say(Npc npc, CommunicationCommandData data)
        {
            transform.position = npc.transform.position + Vector3.up * 3;
            transform.LookAt(Camera.main.transform);
            transform.Rotate(new(0, 180));
            _icon.sprite = data.Icon;
            _text.text = data.Text;

            await Task.Delay(TimeSpan.FromSeconds(_delay));

            if (!destroyCancellationToken.IsCancellationRequested)
                Destroy(gameObject);
        }
    }
}