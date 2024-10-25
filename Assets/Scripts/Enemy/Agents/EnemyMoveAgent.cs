using Components;
using UnityEngine;

namespace ShootEmUp.Agents
{
    public class EnemyMoveAgent : MonoBehaviour
    {
        public bool IsReached => _isReached;
        private bool _isReached;

        [SerializeField]
        private MoveComponent moveComponent;

        private Vector2 _destination;

        public void SetDestination(Vector2 destination)
        {
            _destination = destination;
            _isReached = false;
        }

        public void FixedUpdate()
        {
            if (_isReached)
            {
                return;
            }
            
            var vector = _destination - (Vector2) transform.position;
            if (vector.magnitude <= 0.25f)
            {
                _isReached = true;
                return;
            }

            var direction = vector.normalized * Time.fixedDeltaTime;
            moveComponent.OnMove(direction);
        }
    }
}