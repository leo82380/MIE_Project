using MIE.Manager.Interface;
using UnityEngine;

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
                DontDestroyOnLoad(gameObject);
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

        private void InitializeAllManagers()
        {
            var managers = GetComponentsInChildren<IManager>();
            foreach (var manager in managers)
            {
                managerDatas.AddManager(manager);
            }
        }


        public T GetManager<T>() where T : class, IManager
        {
            return managerDatas.GetManager<T>();
        }
    }
}
