namespace Final_Survivors.Enemies
{
    public class BossRangedEnemy : RangedEnemy
    {
        private void Awake()
        {
            Level = EnemyLevel.BOSS;
        }
    }
}
