using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PerfectScoreMod
{
    internal sealed class PartyCompetition : CompetitionBase
    {
        private LocalLeaderboardsModel _localLeaderboard;
        private List<LocalLeaderboardsModel.ScoreData> _scoreDataCollection;

        protected override void Init()
        {
            string leaderboardID = LeaderboardsModel.GetLeaderboardID(Beatmap);

            // This can return null when no scores exist for said leaderboard.
            IList<LocalLeaderboardsModel.ScoreData> tempCollection = 
                _localLeaderboard.GetScores(leaderboardID, LocalLeaderboardsModel.LeaderboardType.AllTime);
            
            if (tempCollection == null)
            {
                // In the case there is no competition we want to disable the mod.
                LabelHeader.text = string.Empty;
                return;
            }

            TrySubscribeOnScoreDidChangeEvent();

            // This prevents from removing existing scores from the current leaderboard in memory.
            _scoreDataCollection = new List<LocalLeaderboardsModel.ScoreData>();
            _scoreDataCollection.AddRange(tempCollection);

            LeaderboardScoreLabel = GenerateUILabel(
                nameof(LeaderboardScoreLabel),
                ComposeScoreData(_scoreDataCollection.Last(), _scoreDataCollection.Count),
                3,
                LabelPosition + new Vector3(0, -0.3f, 0)
            );
        }

        protected override void OnScoreDidChangeEvent(int currentScore)
        {
            if (_scoreDataCollection.Count == 0)
            {
                LeaderboardScoreLabel.text = "Come at me bro";
                TryUnsubscribeOnScoreDidChangeEvent();
                return;
            }

            _scoreDataCollection.RemoveAll(sData => sData._score <= currentScore);
            LeaderboardScoreLabel.text = ComposeScoreData(_scoreDataCollection.Last(), _scoreDataCollection.Count);
        }

        protected override bool TryResolveChildResources()
        {
            _localLeaderboard = Resources.FindObjectsOfTypeAll<LocalLeaderboardsModel>().FirstOrDefault();

            if (_localLeaderboard != null)
            {
                return true;
            }

            return false;
        }
    }
}
