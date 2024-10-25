using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace ShootEmUp
{
    [UsedImplicitly]
    public class InputSystem:ITickable
    {
        public event Action OnFire;
        public event Action<Vector2> OnMove;
        
        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
                OnFire?.Invoke();

            OnMove?.Invoke(GetDirection());
        }

        private Vector2 GetDirection()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                return Vector2.left*Time.fixedDeltaTime;
            
            if (Input.GetKey(KeyCode.RightArrow))
                return Vector2.right*Time.fixedDeltaTime;
            
            return Vector2.zero;
        }
    }
}