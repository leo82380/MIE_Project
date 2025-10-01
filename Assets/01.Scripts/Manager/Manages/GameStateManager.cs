using System;
using MIE.Manager.Interface;
using MIE.Runtime.EventSystem.Core;
using MIE.Runtime.TimerText;
using EventHandler = MIE.Runtime.EventSystem.Core.EventHandler;

namespace MIE.Manager.Manages
{
    public class GameStateManager : IManager, IInitializable, IDisposable
    {
        private GameState currentState;

        public void Initialize()
        {
            SetState(GameState.None);
            EventHandler.Subscribe<TimerCompleteEvent>(HandleTimerComplete);
        }

        public void Dispose()
        {
            SetState(GameState.None);
            EventHandler.Unsubscribe<TimerCompleteEvent>(HandleTimerComplete);
        }

        private void HandleTimerComplete(TimerCompleteEvent evt)
        {
            SetState(GameState.GameOver);
        }

        public void SetState(GameState newState)
        {
            currentState = newState;
            EventHandler.TriggerEvent(new GameStateEvent(currentState));
        }
    }

    public struct GameStateEvent : IEvent
    {
        public GameState State;

        public GameStateEvent(GameState state)
        {
            State = state;
        }
    }

    public enum GameState
    {
        None,
        GameOver,
        GameClear
    }
}