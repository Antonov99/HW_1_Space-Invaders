using ShootEmUp.Agents;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : MonoBehaviour
    {
        [SerializeField]
        private EnemyAttackAgent attackAgent;
        
        public void Construct(BulletSystem bulletSystem, GameObject target)
        {
            attackAgent.Construct(bulletSystem, target);
        }
    }
}