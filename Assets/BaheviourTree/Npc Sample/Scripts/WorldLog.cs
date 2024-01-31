using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class WorldLog : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textPrefab;
        [SerializeField] private float _duration = 2;
        private static WorldLog _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static async void Log(Vector3 point, string message, Component sender = null)
        {
            if (sender == null)
                Debug.Log(message);
            else
                Debug.Log($"{sender.name} : {message}");

            var text = Instantiate(_instance._textPrefab, point + Vector3.up * 3, Quaternion.identity);
            text.transform.LookAt(Camera.main.transform);
            text.transform.Rotate(new(0, 180));
            text.text = message;
            await Task.Delay(TimeSpan.FromSeconds(_instance._duration));
            Destroy(text.gameObject);
        }
    }
}