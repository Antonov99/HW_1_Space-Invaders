using System;
using Components;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {
        public event Action<Bullet> OnCollisionEntered;

        [NonSerialized]
        public bool isPlayer;

        [NonSerialized]
        public int damage;

        [SerializeField]
        public new Rigidbody2D rigidbody2D;

        [SerializeField]
        public SpriteRenderer spriteRenderer;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(this);
            if (!collision.gameObject.TryGetComponent(out TeamComponent teamComponent)) return;
            if(teamComponent.IsPlayer==isPlayer) return;
            collision.gameObject.GetComponent<HealthComponent>().TakeDamage(damage);
        }
    }
}