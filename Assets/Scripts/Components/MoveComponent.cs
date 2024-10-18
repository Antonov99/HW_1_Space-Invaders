using UnityEngine;

namespace Components
{
    public class MoveComponent:MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody2D rigidbody;

        [SerializeField]
        private float speed = 5.0f;

        public void OnMove(Vector2 moveDirection)
        {
            var nextPosition = rigidbody.position + moveDirection * speed;
            rigidbody.MovePosition(nextPosition);
        }
    }
}