using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using BeatSaberMods.Shared;

namespace ProfilesMod
{
    internal class MissedCounter : MonoBehaviour
    {
        private int _counter = 0;
        private ScoreController _scoreController;

        private GameObject _countGameObject;
        private TextMeshPro _counterText;

        private async void Awake()
        {
            await GetScore();
        }

        private Task GetScore()
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    _scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault();

                    if (_scoreController != null)
                    {
                        break;
                    }
                    Thread.Sleep(10);
                }

                Init();
            });
        }

        private void Init()
        {
            _counterText = gameObject.AddComponent<TextMeshPro>();
            _counterText.text = "0";
            _counterText.fontSize = 4;
            _counterText.color = Color.white;
            _counterText.font = Resources.Load<TMP_FontAsset>(Constants.BeonNoGlow);
            _counterText.alignment = TextAlignmentOptions.Center;
            _counterText.rectTransform.position = Plugin.counterPosition + new Vector3(0, -0.4f, 0);

            _countGameObject = new GameObject("Label");

            TextMeshPro label = _countGameObject.AddComponent<TextMeshPro>();
            label.text = "Misses";
            label.fontSize = 3;
            label.color = Color.white;
            label.font = Resources.Load<TMP_FontAsset>(Constants.BeonNoGlow);
            label.alignment = TextAlignmentOptions.Center;
            label.rectTransform.position = Plugin.counterPosition;

            if (_scoreController != null)
            {
                _scoreController.noteWasCutEvent += OnNoteCut;
                _scoreController.noteWasMissedEvent += OnNoteMissed;
            }
        }

        private void OnNoteCut(NoteData noteData, NoteCutInfo noteInfo, int c)
        {
            if (noteData.noteType == NoteType.Bomb || !noteInfo.allIsOK)
            {
                IncrementCounter();
            }
        }

        private void OnNoteMissed(NoteData noteData, int c)
        {
            if (noteData.noteType != NoteType.Bomb)
            {
                IncrementCounter();
            }
        }

        private void IncrementCounter()
        {
            _counter++;
            _counterText.text = _counter.ToString();
        }

        private void OnDestroy()
        {
            if (_scoreController != null)
            {
                _scoreController.noteWasCutEvent -= OnNoteCut;
                _scoreController.noteWasMissedEvent -= OnNoteMissed;
            }
        }
    }
}
