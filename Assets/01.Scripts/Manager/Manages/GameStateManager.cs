using System;
using MIE.Manager.Interface;
using MIE.Runtime.BoardSystem;
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
            EventHandler.Subscribe<BoardClearedEvent>(HandleBoardCleared);
        }

        public void Dispose()
        {
            SetState(GameState.None);
            EventHandler.Unsubscribe<TimerCompleteEvent>(HandleTimerComplete);
            EventHandler.Unsubscribe<BoardClearedEvent>(HandleBoardCleared);
        }

        private void HandleBoardCleared(BoardClearedEvent evt)
        {
            SetState(GameState.GameClear);
        }

        private void HandleTimerComplete(TimerCompleteEvent evt)
        {
            SetState(GameState.GameOver);
        }

        public void SetState(GameState newState)
        {
            currentState = newState;
            EventHandler.TriggerEvent(GameStateEvent.Create(currentState));
        }
    }

    public struct GameStateEvent : IEvent
    {
        public GameState State;

        public GameStateEvent(GameState state)
        {
            State = state;
        }

        private static GameStateEvent instance;

        public static GameStateEvent Create(GameState state)
        {
            instance.State = state;
            return instance;
        }
    }

    public enum GameState
    {
        None,
        GameOver,
        GameClear
    }
}