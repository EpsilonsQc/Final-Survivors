namespace Final_Survivors.Enemies
{
    public class BossMeleeEnemy : MeleeEnemy
    {
        private void Awake()
        {
            Level = EnemyLevel.BOSS;
        }
    }
}
