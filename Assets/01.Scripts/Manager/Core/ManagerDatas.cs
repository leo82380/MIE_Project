using System;
using System.Collections.Generic;
using System.Linq;
using MIE.Manager.Interface;
using UnityEngine;

namespace MIE.Manager.Core
{
    public class ManagerDatas
    {
        private Dictionary<Type, IManager> managers = new();

        public Dictionary<Type, IManager> Managers => managers;

        public void AddManager(IManager manager)
        {
            var type = manager.GetType();
            if (managers.ContainsKey(type))
            {
                Debug.LogWarning($"Manager of type {type} already exists.");
                return;
            }
            managers[type] = manager;

            if (manager is IInitializable initializable)
                initializable.Initialize();
        }

        public T GetManager<T>() where T : class, IManager
        {
            var type = typeof(T);
            if (managers.TryGetValue(type, out var manager))
                return manager as T;

            Debug.LogWarning($"Manager of type {type} not found.");
            return null;
        }

        public List<IManager> GetAllManagers()
        {
            return managers.Values.ToList();
        }
    }
}
