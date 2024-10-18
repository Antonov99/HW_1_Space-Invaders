
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Player : MonoBehaviour
    {
        [SerializeField]
        public bool isPlayer;
        
        [SerializeField]
        public Transform firePoint;
        
        [SerializeField]
        public int health;

        [SerializeField]
        public Rigidbody2D _rigidbody;

        [SerializeField]
        public float speed = 5.0f;
    }
}