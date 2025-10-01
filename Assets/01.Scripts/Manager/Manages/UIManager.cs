using System;
using System.Linq;
using System.Reflection;
using MIE.Manager.Interface;
using MIE.UI.PopupSystem;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = MIE.Runtime.EventSystem.Core.EventHandler;
using MIE.UI.FadeSystem;
using MIE.Runtime.EventSystem.Core;

namespace MIE.Manager.Manages
{
    public class UIManager : IManager, IInitializable, IDisposable
    {
        private Dictionary<Type, IUIModule> uiModules = new Dictionary<Type, IUIModule>();

        public void Initialize()
        {
            RegisterAllUIModules();
            
            EventHandler.Subscribe<GameStateEvent>(HandleStateChange);
        }

        private void RegisterAllUIModules()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var uiModuleTypes = assembly.GetTypes()
                .Where(type => 
                    typeof(IUIModule).IsAssignableFrom(type) && 
                    !type.IsInterface && 
                    !type.IsAbstract)
                .ToList();

            Debug.Log($"[UIManager] Found {uiModuleTypes.Count} UI Module types");

            foreach (var moduleType in uiModuleTypes)
            {
                try
                {
                    var moduleInstance = (IUIModule)Activator.CreateInstance(moduleType);
                    moduleInstance.Initialize();
                    uiModules[moduleType] = moduleInstance;
                    
                    Debug.Log($"[UIManager] Registered UI Module: {moduleType.Name}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[UIManager] Failed to register UI Module {moduleType.Name}: {ex.Message}");
                }
            }

            Debug.Log($"[UIManager] UI Module registration completed. Total registered: {uiModules.Count}");
        }

        public void Dispose()
        {
            foreach (var module in uiModules.Values)
            {
                try
                {
                    module?.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[UIManager] Error disposing UI Module: {ex.Message}");
                }
            }
            
            uiModules.Clear();
            EventHandler.Unsubscribe<GameStateEvent>(HandleStateChange);
        }

        private void HandleStateChange(GameStateEvent t)
        {
            if (t.State == GameState.GameOver)
            {
                GetUIModule<FadeModule>().OpenPanel("Image_BG");
                GetUIModule<PopupModule>().OpenPopup(PopupType.GameOver);
            }
            else if (t.State == GameState.GameClear)
            {
                GetUIModule<FadeModule>().OpenPanel("Image_BG");
                GetUIModule<PopupModule>().OpenPopup(PopupType.GameClear);
            }

            EventHandler.TriggerEvent<GameEndEvent>();
        }

        public T GetUIModule<T>() where T : IUIModule
        {
            var type = typeof(T);
            if (uiModules.TryGetValue(type, out var module))
            {
                return (T)module;
            }
            else
            {
                throw new Exception($"[UIManager] UI Module of type {type} not found.");
            }
        }
    }

    public struct GameEndEvent : IEvent
    {
        private static GameEndEvent instance = new GameEndEvent();

        public static GameEndEvent Create()
        {
            return instance;
        }
    }
}