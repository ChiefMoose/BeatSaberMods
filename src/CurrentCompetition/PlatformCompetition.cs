using BeatSaberMods.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static PlatformLeaderboardsModel;

namespace PerfectScoreMod
{
    class PlatformCompetition : CompetitionBase
    {
        private PlatformLeaderboardsModel _platformLeaderboardsModel;
        private TextMeshPro _leaderboardScore;

        protected override void Init()
        {
            // I can't get this to work correctly yet.
            //GetScoresCompletionHandler m = new GetScoresCompletionHandler(GetScoresResult.OK, LeaderboardScore[], );
            //_platformLeaderboardsModel.GetScoresAroundPlayer(Beatmap, 10, );

            _leaderboardScore = GenerateUILabel(
                "LeaderboardScore",
                "Loaded Properly",
                3,
                LabelPosition + new Vector3(0, -0.3f, 0)
            );
        }

        protected override void OnScoreDidChangeEvent(int currentScore)
        {
            
        }

        protected override bool TryResolveChildResources()
        {
            _platformLeaderboardsModel = Resources.FindObjectsOfTypeAll<PlatformLeaderboardsModel>().FirstOrDefault();

            if (_platformLeaderboardsModel != null)
            {
                return true;
            }

            return false;
        }
    }
}
