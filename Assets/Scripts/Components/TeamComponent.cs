using UnityEngine;

namespace Components
{
    public sealed class TeamComponent : MonoBehaviour
    {
        public bool IsPlayer => isPlayer;

        [SerializeField]
        private bool isPlayer;
    }
}