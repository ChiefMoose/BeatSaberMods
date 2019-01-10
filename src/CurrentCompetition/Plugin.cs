using BeatSaberMods.Shared;
using IllusionPlugin;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PerfectScoreMod
{
    public class Plugin : IPlugin
    {
        public string Name => "PerfectScoreMod";
        public string Version => "0.0.1";

        private bool _enabled = true;

        public void OnApplicationStart()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            if (!_enabled)
            {
                return;
            }

            if (!arg1.name.Equals("GameCore"))
            {
                return;
            }

            PartyFreePlayFlowCoordinator partyFlowCoordinator = Resources.FindObjectsOfTypeAll<PartyFreePlayFlowCoordinator>().FirstOrDefault();
            if (partyFlowCoordinator != null && partyFlowCoordinator.isActivated)
            {
                arg1.TryAddGameCoreComponent<PartyCompetition>();
                return;
            }

            SoloFreePlayFlowCoordinator soloFlowCoordinator = Resources.FindObjectsOfTypeAll<SoloFreePlayFlowCoordinator>().FirstOrDefault();
            if (soloFlowCoordinator != null && soloFlowCoordinator.isActivated)
            {
                arg1.TryAddGameCoreComponent<PlatformCompetition>();
                return;
            }
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
        }

        public void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        }

        public void OnLevelWasLoaded(int level)
        {

        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}
