using UnityEngine;

namespace Enemy.Core
{
    [CreateAssetMenu(menuName = "AI/Attack Pattern")]
    public class AttackPatternSO : ScriptableObject
    {
        [SerializeField] private EnemyAttackSO[] attacks;
        [SerializeField] private AttackSelectionType selectionType;


        private int _lastIndex = -1;

        public EnemyAttackSO PickNext(Vector3 selfPos, Vector3 playerPos)
        {
            switch (selectionType)
            {
                case AttackSelectionType.Random:
                    return attacks[Random.Range(0, attacks.Length)];
                case AttackSelectionType.Sequential:
                    _lastIndex = (_lastIndex + 1) % attacks.Length;
                    return attacks[_lastIndex];
                case AttackSelectionType.ClosestByDistance:
                    var dist = Vector3.Distance(selfPos, playerPos);
                    foreach (var atk in attacks)
                    {
                        if (dist >= atk.minDistance && dist <= atk.maxDistance)
                            return atk;
                    }

                    return null;
                default:
                    return attacks[0];
            }
        }

        private enum AttackSelectionType
        {
            Random,
            Sequential,
            ClosestByDistance
        }
    }
}