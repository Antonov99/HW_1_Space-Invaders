using System;
using JetBrains.Annotations;
using Modules;
using UnityEngine;
using Zenject;

namespace InputSystem
{
    [UsedImplicitly]
    public class InputAdapter : ITickable
    {
        public event Action<SnakeDirection> OnMove;

        public void Tick()
        {
            OnMove?.Invoke(GetDirection());
        }

        private SnakeDirection GetDirection()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                return SnakeDirection.LEFT;

            if (Input.GetKey(KeyCode.RightArrow))
                return SnakeDirection.RIGHT;

            if (Input.GetKey(KeyCode.UpArrow))
                return SnakeDirection.UP;

            if (Input.GetKey(KeyCode.DownArrow))
                return SnakeDirection.DOWN;

            return SnakeDirection.NONE;
        }
    }
}