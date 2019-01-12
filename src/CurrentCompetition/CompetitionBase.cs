using BeatSaberMods.Shared;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace PerfectScoreMod
{
    internal abstract class CompetitionBase : MonoBehaviour
    {
        /// <summary>
        /// The base position of the Leaderboard label.
        /// </summary>
        protected Vector3 LabelPosition { get { return _labelPosition; } }
        private Vector3 _labelPosition = new Vector3(3.25f, 0.2f, 7f);
        
        /// <summary>
        /// Score controller used to subscribe to required events.
        /// </summary>
        private ScoreController ScrController { get; set; }

        protected IDifficultyBeatmap Beatmap { get; private set; }

        protected TextMeshPro LabelHeader { get; set; }

        protected abstract void Init();
        protected abstract bool TryResolveChildResources();
        protected abstract void OnScoreDidChangeEvent(int currentScore);

        /// <summary>
        /// UI Label element that displays the current competing score.
        /// </summary>
        protected TextMeshPro LeaderboardScoreLabel { get; set; }

        private async void Awake()
        {
            await GetLeaderboardScore();
        }

        private void OnDestroy()
        {
            TryUnsubscribeOnScoreDidChangeEvent();
        }

        private Task GetLeaderboardScore()
        {
            return Task.Run(() =>
            {
                bool canResolveResources = false;
                while (!canResolveResources)
                {
                    // Try to resolve the required resources.
                    canResolveResources = TryResolveResources();
                }

                LabelHeader = GenerateUILabel("Label", "Current Competition", 2, _labelPosition);

                Init();
            });
        }

        /// <summary>
        /// Tries to subscribe to the <seealso cref="ScoreController.scoreDidChangeEvent"/>.
        /// </summary>
        /// <returns>Returns true if we are able to successfully subscribe. Otherwise returns false.</returns>
        protected bool TrySubscribeOnScoreDidChangeEvent()
        {
            if (ScrController == null)
            {
                return false;
            }
            ScrController.scoreDidChangeEvent += OnScoreDidChangeEvent;
            return true;
        }

        /// <summary>
        /// Tries to unsubscribe to the <seealso cref="ScoreController.scoreDidChangeEvent"/>.
        /// </summary>
        /// <returns>Returns true if we are able to successfully unsubscribe. Otherwise returns false.</returns>
        protected bool TryUnsubscribeOnScoreDidChangeEvent()
        {
            if (ScrController == null)
            {
                return false;
            }

            ScrController.scoreDidChangeEvent -= OnScoreDidChangeEvent;
            return true;
        }

        /// <summary>
        /// Creates a new UI Label used to display competition data.
        /// </summary>
        /// <param name="name">Name of the label</param>
        /// <param name="text">Display data of the label</param>
        /// <param name="fontSize">Size of the text</param>
        /// <param name="position">Position of the label</param>
        /// <returns>Returns the newly created label object.</returns>
        protected TextMeshPro GenerateUILabel(string name, string text, int fontSize, Vector3 position)
        {
            GameObject labelGameObject = new GameObject(name);

            TextMeshPro label = labelGameObject.AddComponent<TextMeshPro>();
            label.text = text;
            label.fontSize = fontSize;
            label.color = Color.white;
            label.font = Resources.Load<TMP_FontAsset>(Constants.BeonNoGlow);
            label.alignment = TextAlignmentOptions.Center;
            label.rectTransform.position = position;

            return label;
        }

        /// <summary>
        /// Creates the Text information of the current competition.
        /// <para>
        /// Formatted string output: {Position}:{PlayerName} - {Score}.
        /// </para>
        /// </summary>
        /// <param name="scoreData">ScoreData in which we are competing</param>
        /// <param name="position">Position in the leaderboard of the current competition</param>
        /// <returns>Returns formatted string if ScoreData is not null. Otherwise returns "N/A".</returns>
        protected virtual string ComposeScoreData(LocalLeaderboardsModel.ScoreData scoreData, int position)
        {
            if (scoreData == null)
            {
                return "N/A";
            }

            return $"{position}:{scoreData._playerName} - {scoreData._score}";
        }

        private bool TryResolveResources()
        {
            ScrController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault();
            Beatmap = Resources.FindObjectsOfTypeAll<StandardLevelDetailViewController>().FirstOrDefault().difficultyBeatmap;

            // Resolve any addiitonal resources that may be required for child types.
            if (ScrController != null && TryResolveChildResources())
            {
                return true;
            }
            
            Thread.Sleep(10);
            return false;
        }
    }
}
