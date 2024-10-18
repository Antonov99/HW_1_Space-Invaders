using System;
using UnityEngine;

namespace Components
{
    public class HealthComponent:MonoBehaviour
    {
        public event Action<GameObject> OnHpEmpty;
        
        [SerializeField] 
        private int health;
        
        public bool IsHitPointsExists() 
        {
            return health > 0;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                OnHpEmpty?.Invoke(gameObject);
            }
        }
    }
}