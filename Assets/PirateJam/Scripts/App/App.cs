using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PirateJam.Scripts.App
{
    /// <summary>
    /// Class that affords the attached prefab lifetime throughout the application
    /// </summary>
    public static class App
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            var app = Object.Instantiate(Resources.Load("App")) as GameObject;
            if (app == null)
                throw new ApplicationException();

            Object.DontDestroyOnLoad(app);
        }
    }
}