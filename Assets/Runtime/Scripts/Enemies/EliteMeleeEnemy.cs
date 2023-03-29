namespace Final_Survivors.Enemies
{
    public class EliteMeleeEnemy : MeleeEnemy
    {
        private void Awake()
        {
            Level = EnemyLevel.ELITE;
        }
    }
}
