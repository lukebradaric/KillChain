using System;
using System.Reflection;
using UnityEngine;

namespace KillChain.Core.Bootstrap
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttributes(typeof(AutoBootstrapAttribute), true).Length > 0)
                    {
                        // Don't instantiate if object already exists
                        if (UnityEngine.Object.FindObjectOfType(type) != null)
                        {
                            Debug.LogWarning($"{type.Name} already exists.");
                            continue;
                        }

                        // Don't instantiate if type is not a monobehaviour
                        if (!type.IsSubclassOf(typeof(MonoBehaviour)))
                        {
                            Debug.LogError($"{nameof(AutoBootstrapAttribute)} cannot be applied to non-MonoBehaviour. ({type.Name})");
                            continue;
                        }

                        // Instantiate gameobject
                        GameObject gameObject = new GameObject(type.Name);
                        gameObject.AddComponent(type);

                        // Mark gameObject to not be destroyed
                        GameObject.DontDestroyOnLoad(gameObject);
                    }
                }
            }

            Debug.Log("Bootstrap completed.");
        }
    }

}
