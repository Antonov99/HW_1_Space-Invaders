using ShootEmUp;
using UnityEngine;
using Zenject;

namespace Components
{
    public class MoveComponent:MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody2D rigidbody;

        [SerializeField]
        private float speed = 5.0f;

        private LevelBounds _levelBounds;

        [Inject]
        public void Construct(LevelBounds levelBounds)
        {
            _levelBounds = levelBounds;
        }

        public void OnMove(Vector2 moveDirection)
        {
            var nextPosition = rigidbody.position + moveDirection * speed;
            if(!_levelBounds.InBounds(nextPosition)) return;
            rigidbody.MovePosition(nextPosition);
        }
    }
}