using BeatSaberMods.Shared;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProfilesMod
{
    public class Plugin : IPlugin
    {
        public string Name => "ProfilesMod";
        public string Version => "0.0.1";

        private bool _enabled = true;

        public static Vector3 counterPosition = new Vector3(-3.25f, 3.0f, 7f);

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

            arg1.TryAddGameCoreComponent<MissedCounter>();
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
