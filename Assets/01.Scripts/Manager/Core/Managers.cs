using System;
using System.Linq;
using System.Reflection;
using MIE.Manager.Interface;
using UnityEngine;
using EventHandler = MIE.Runtime.EventSystem.Core.EventHandler;

namespace MIE.Manager.Core
{
    public class Managers : MonoBehaviour
    {
        private static Managers instance;
        private ManagerDatas managerDatas = new();

        public static Managers Instance => instance;
        public ManagerDatas ManagerDatas => managerDatas;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (this != instance)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            InitializeAllManagers();
        }

        private void OnDestroy()
        {
            DisposeAllManagers();
        }

        private void InitializeAllManagers()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var managerTypes = assembly.GetTypes()
                .Where(type => typeof(IManager).IsAssignableFrom(type) &&
                              !type.IsInterface &&
                              !type.IsAbstract)
                .ToArray();

            foreach (var managerType in managerTypes)
            {
                try
                {
                    IManager manager = null;

                    if (typeof(MonoBehaviour).IsAssignableFrom(managerType))
                    {
                        manager = GetComponentInChildren(managerType) as IManager;

                        if (manager == null)
                        {
                            var component = gameObject.AddComponent(managerType);
                            manager = component as IManager;
                            Debug.Log($"[Managers] Added component: {managerType.Name}");
                        }
                        else
                        {
                            Debug.Log($"[Managers] Found existing component: {managerType.Name}");
                        }
                    }
                    else
                    {
                        if (managerType.GetConstructor(Type.EmptyTypes) != null)
                        {
                            manager = Activator.CreateInstance(managerType) as IManager;
                            Debug.Log($"[Managers] Created new instance: {managerType.Name}");
                        }
                        else
                        {
                            Debug.LogWarning($"[Managers] {managerType.Name} has no parameterless constructor");
                            continue;
                        }
                    }

                    if (manager != null)
                    {
                        managerDatas.AddManager(manager);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[Managers] Failed to initialize {managerType.Name}: {ex.Message}");
                }
            }
        }

        private void DisposeAllManagers()
        {
            foreach (var manager in managerDatas.GetAllManagers())
            {
                if (manager is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            EventHandler.Clear();
        }


        public T GetManager<T>() where T : class, IManager
        {
            return managerDatas.GetManager<T>();
        }
    }
}
