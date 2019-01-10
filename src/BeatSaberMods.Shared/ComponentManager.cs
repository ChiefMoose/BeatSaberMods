using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeatSaberMods.Shared
{
    /// <summary>
    /// Extension methods for the UnityEngine.Scene class.
    /// </summary>
    public static class SceneExtensionMethods
    {
        private const string GameCore = "GameCore";
        private const string EmptyTransition = "EmptyTransition";

        /// <summary>
        /// Tries to add component of <typeparamref name="T"/> if the name of the current scene matched "GameCore".
        /// </summary>
        /// <typeparam name="T">Type of component to be added to the current scene.</typeparam>
        /// <param name="currentScene">The current scene.</param>
        /// <param name="isEnabled">The state of the component being added.</param>
        /// <returns>If the component is added to the scene, returns true. Otherwise returns false.</returns>
        public static bool TryAddGameCoreComponent<T>(this Scene currentScene) where T : Component
        {
            new GameObject(nameof(T)).AddComponent<T>();

            bool locatedComponent = currentScene.GetRootGameObjects().Any(gObject => gObject.name.Equals(nameof(T)));
            if (!locatedComponent)
            {
                SimpleFileLogger.Log($"ERR: Unable to add Component {nameof(T)}");
            }

            return locatedComponent;
        }
    }
}
