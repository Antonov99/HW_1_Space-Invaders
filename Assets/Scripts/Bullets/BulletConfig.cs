using UnityEngine;

namespace ShootEmUp
{
    [CreateAssetMenu(
        fileName = "BulletConfig",
        menuName = "Bullets/New BulletConfig"
    )]
    public class BulletConfig:ScriptableObject
    {
        [SerializeField]
        public PhysicsLayer physicsLayer;

        [SerializeField]
        public Color color;

        [SerializeField]
        public int damage;

        [SerializeField]
        public float speed;
    }
}